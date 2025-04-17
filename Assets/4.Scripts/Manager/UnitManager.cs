using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class UnitManager : MonoBehaviour
{
    [Header ("System")]
    [SerializeField] private GameObject Marker;
    [SerializeField] private GameObject NameText;
    [SerializeField] private GameObject HoldingMaterialManager;
    [SerializeField] private UnitBaseData unitBaseData;
    [SerializeField] private LayerMask layerEnemy;

    [Header ("Other")]
    public Coroutine coroutineList;
    public ButtonPageNum nowButtonNumList;

    [Header ("State")]
    public ObjectState objectState;
    public int uiPriority;
    public bool canAttack;
    public bool canMagic;

    [Header ("Collision")]
    public GameObject targetObject; //타겟 오브젝트
    public Vector3 targetEnd;       //타겟 좌표
    public float isCollisionTimer;  //충돌 시간
    public bool isCollisionTarget;  //타겟과의 충돌여부
    public bool isCollisionObject;  //타 오브젝트와의 충돌여부     

    [Header("Status")]
    public int maxHp;               //최대 체력
    public int nowHp;               //현재 체력
    public int nowDamage;           //현재 공격력
    public int nowDefence;          //현재 방어력
    public int nowMoveSpeed;        //현재 이동 속도
    public int nowEnergy;           //현재 에너지

    private NavMeshAgent m_NavMestAgent;

    private void Awake()
    {
        m_NavMestAgent = GetComponent<NavMeshAgent>();

        Marker.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        Marker.transform.localPosition = new Vector3(0, -transform.localScale.y / 2 + 0.01f, 0);
        Marker.SetActive(false);

        NameText.transform.localPosition = new Vector3(0, transform.localScale.y / 2, 0);
        if(unitBaseData.objectSize == ObjectSize.Small) { NameText.transform.localScale = new Vector3(2f, 2f, 1f); }
        NameText.transform.rotation = Quaternion.Euler(90, 0, 0);
        
        objectState = ObjectState.Stop;
        m_NavMestAgent.avoidancePriority = 30;
        canAttack = true;
        isCollisionTimer = 0f;
        isCollisionTarget = false;
        isCollisionObject = false;

        nowButtonNumList = unitBaseData.mainButtonPage;

        if (unitBaseData.isAttack == true) { StartCoroutine(AttackCoolTimeCoroutine()); }
        if(unitBaseData.isMagic == true) { StartCoroutine(ManaRegenCoroutine()); }

        SetHp();
    }
    public void MarkedUnit() // 유닛 선택 시 마크 표시
    {
        Marker.SetActive(true);
        nowButtonNumList = unitBaseData.mainButtonPage;
    }
    public void UnMarkedUnit() // 유닛 선택 해제 시 마크 비표시
    {
        Marker.SetActive(false);
        nowButtonNumList = unitBaseData.mainButtonPage;
    }
    public void StopMove() // 정지
    {
        m_NavMestAgent.ResetPath();
        m_NavMestAgent.avoidancePriority = 40;
        m_NavMestAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        targetObject = default;
        targetEnd = default;
        m_NavMestAgent.velocity = Vector3.zero;
        
        objectState = ObjectState.Stop;
    }
    #region Move
    public IEnumerator MoveCoroutine(Vector3 End) // 유닛 이동(땅 클릭)
    {
        float Timer = 0f;
        objectState = ObjectState.Move;
        m_NavMestAgent.avoidancePriority = 50;
        m_NavMestAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        m_NavMestAgent.stoppingDistance = SetStoppingDistance();
        targetEnd = End;
        targetEnd.y = 0.5f;

        while (objectState == ObjectState.Move)
        {  
            m_NavMestAgent.speed = unitBaseData.moveSpeed * Time.deltaTime;

            m_NavMestAgent.SetDestination(targetEnd);
            Timer += 0.1f;

            if (Timer >= 2f)
            {
                if (IsBlocked()||IsArrived())
                {
                    StopMove();
                    yield break;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator MoveCoroutine(GameObject target) //유닛 이동(오브젝트 클릭)
    {
        if(target == this.gameObject)
        {
            StopMove();
            yield break;
        }
        float Timer = 0f;
        objectState = ObjectState.Move;
        m_NavMestAgent.avoidancePriority = 50;
        m_NavMestAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        m_NavMestAgent.stoppingDistance = SetStoppingDistance();

        while (objectState == ObjectState.Move)
        {
            m_NavMestAgent.speed = unitBaseData.moveSpeed * Time.deltaTime;

            targetObject = target;
            targetEnd = (SetTargetEnd(targetObject));
            m_NavMestAgent.SetDestination(targetEnd);

            Timer += 0.1f;

            if (Timer >= 2f)
            {
                if (IsBlocked() || IsArrived())
                {
                    transform.LookAt(targetObject.transform);
                    StopMove();
                    yield break;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion 
    #region Hold
    public IEnumerator HoldCoroutine() //홀드
    {
        m_NavMestAgent.ResetPath();
        m_NavMestAgent.avoidancePriority = 30;
        m_NavMestAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        objectState = ObjectState.Hold;
        while (objectState == ObjectState.Hold)
        {
            Collider[] arroundEnemy = Physics.OverlapSphere(transform.position, unitBaseData.attackRange + transform.lossyScale.x, layerEnemy);
            if (arroundEnemy.Length > 0)
            {
                GameObject targetEnemy;
                targetEnemy = ShortestEnemy(arroundEnemy);
                transform.LookAt(targetEnemy.transform);
                if (canAttack == true)
                {
                    targetEnemy.GetComponent<EnemyManager>().TakeDamage(unitBaseData.baseDamage, unitBaseData.attackType);
                    canAttack = false;
                }
                else if (canAttack == false)
                {
                    Debug.Log("AttackCoolTime");
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
    #region Patrol
    public IEnumerator PatrolCoroutine(Vector3 end) // 패트롤
    {
        float Timer = 0f;
        objectState = ObjectState.Patrol;
        m_NavMestAgent.avoidancePriority = 50;
        m_NavMestAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        m_NavMestAgent.stoppingDistance = SetStoppingDistance();
        targetEnd.y = 0.5f;
        Vector3 targetA = end;
        Vector3 targetB = transform.position;
        bool startToEnd = true;

        while(objectState == ObjectState.Patrol)
        {
            if (startToEnd == true) { targetEnd = targetA; }
            else { targetEnd = targetB; }
            m_NavMestAgent.speed = unitBaseData.moveSpeed * Time.deltaTime;

            m_NavMestAgent.SetDestination(targetEnd);
            Timer += 0.1f;

            if (Timer >= 2f)
            {
                if (IsBlocked())
                {
                    StopMove();
                    yield break;
                }
            }
            if(IsArrived())
            {
                if(startToEnd == true) { startToEnd = false; }
                else {  startToEnd = true; }
                yield return new WaitForSecondsRealtime(0.1f);
            }
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
    #region attack
    public IEnumerator AttackCoroutine(Vector3 End) //유닛 공격(땅 클릭)
    {
        m_NavMestAgent.avoidancePriority = 45;
        m_NavMestAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        m_NavMestAgent.stoppingDistance = 0;
        float Timer = 0f;

        while (objectState == ObjectState.Attack)
        {
            objectState = ObjectState.Attack;
            m_NavMestAgent.speed = unitBaseData.moveSpeed * Time.deltaTime;
            Timer += 0.1f;
            GameObject targetEnemy = null;
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2f + unitBaseData.attackRange, layerEnemy);

            if (colliders.Length > 0 || targetEnemy != null)
            {
                targetEnemy = ShortestEnemy(colliders);
                EnemyManager targetEnemyComponent = targetEnemy.GetComponent<EnemyManager>();


                m_NavMestAgent.stoppingDistance = SetAttackStoppingDistance();
                targetObject = targetEnemy;
                targetEnd = (SetTargetEnd(targetObject));
                m_NavMestAgent.SetDestination(targetEnd);

                if (Timer >= 2f)
                {
                    if ((targetEnemy.GetComponent<EnemyManager>().objectState == ObjectState.Destroy) || IsBlocked())
                    {
                        StopMove();
                        yield break;
                    }
                }
                if(IsArrived())
                {
                    if (canAttack == true)
                    {
                        transform.LookAt(targetObject.transform);
                        targetEnemyComponent.TakeDamage(unitBaseData.baseDamage, unitBaseData.attackType);
                        canAttack = false;
                    }
                    else if (canAttack == false)
                    {
                        Debug.Log("AttackCoolTime");
                    }
                }
            }
            else
            {
                m_NavMestAgent.stoppingDistance = 0;
                m_NavMestAgent.stoppingDistance = SetStoppingDistance();
                targetEnd = End;
                targetEnd.y = 0.5f;
                m_NavMestAgent.SetDestination(targetEnd);

                if (Timer >= 2f)
                {
                    if (IsBlocked() || IsArrived())
                    {
                        StopMove();
                        yield break;
                    }
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator AttackCoroutine(GameObject targetEnemy) //유닛 공격(오브젝트 클릭)
    {
        float Timer = 0f;
        objectState = ObjectState.Attack;
        m_NavMestAgent.avoidancePriority = 45;
        m_NavMestAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        targetObject = targetEnemy;
        if (!targetEnemy.CompareTag("Enemy") || CorrectAirGround(targetEnemy) == false) { StopMove(); yield break; }
        EnemyManager targetEnemyComponent = targetEnemy.GetComponent<EnemyManager>();

        while (objectState == ObjectState.Attack)
        {
            if (!unitBaseData.isAttack || targetObject == null) { StopMove(); yield break; }
            m_NavMestAgent.speed = unitBaseData.moveSpeed * Time.deltaTime;
            m_NavMestAgent.stoppingDistance = SetAttackStoppingDistance();
            targetEnd = (SetTargetEnd(targetObject));
            m_NavMestAgent.SetDestination(targetEnd);
            Timer += 0.1f;

            if (Timer >= 2f)
            {
                if ((targetEnemyComponent.objectState == ObjectState.Destroy) || IsBlocked())
                {
                    StopMove();
                    yield break;
                }
            }
            if (IsArrived())
            {
                if (canAttack == true)
                {
                    transform.LookAt(targetObject.transform);
                    targetEnemyComponent.TakeDamage(nowDamage, unitBaseData.attackType);
                    canAttack = false;
                }

                else if (canAttack == false)
                {
                    Debug.Log("AttackCoolTime");
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator AttackCoolTimeCoroutine() // 공격 쿨타임
    {
        while (true)
        {
            if (canAttack == false)
            {
                canAttack = true;
                yield return new WaitForSecondsRealtime(unitBaseData.attackSpeed);
            }
            else if(canAttack == true)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public float AttackTypeUnitSize(AttackType attackType, ObjectSize unitSize) // 유닛 사이즈, 공격 방식 계산
    {

        if (unitSize == ObjectSize.Small)
        {
            if (attackType == AttackType.Normal) { return 1f; }
            else if (attackType == AttackType.Explosive) { return 0.5f; }
            else if (attackType == AttackType.Concussive) { return 1f; }
            else if (attackType == AttackType.Spell) { return 1f; }
        }
        else if (unitSize == ObjectSize.Medium)
        {
            if (attackType == AttackType.Normal) { return 1f; }
            else if (attackType == AttackType.Explosive) { return 0.75f; }
            else if (attackType == AttackType.Concussive) { return 0.5f; }
            else if (attackType == AttackType.Spell) { return 1f; }
        }
        else if (unitSize == ObjectSize.Large)
        {
            if (attackType == AttackType.Normal) { return 1f; }
            else if (attackType == AttackType.Explosive) { return 1f; }
            else if (attackType == AttackType.Concussive) { return 0.25f; }
            else if (attackType == AttackType.Spell) { return 1f; }
        }
        return 0f;
    }

    public bool CorrectAirGround(GameObject target) // 공중,지상 공격 가능 여부
    {
        if (unitBaseData.attackAirGround == AttackAirGround.AirGround) { return true; }

        if (target.GetComponent<EnemyManager>().GetData().airGround == AirGround.Air)
        {

            if(unitBaseData.attackAirGround == AttackAirGround.Air) {  return true; }
            else if(unitBaseData.attackAirGround == AttackAirGround.Ground) { return false; }
        }
        else if(target.GetComponent<EnemyManager>().GetData().airGround == AirGround.Ground)
        {
            if (unitBaseData.attackAirGround == AttackAirGround.Air) { return false; }
            else if (unitBaseData.attackAirGround == AttackAirGround.Ground) { return true; }
        }

        //debug
        Debug.Log("AirGround Error"); 
        return false;
    }
    public GameObject ShortestEnemy(Collider[] colliders) // 가장 가까운 적 찾기
    {
        Collider shortestEnemy = colliders[0];
        float shortDistance = Vector3.Distance(transform.position, colliders[0].transform.position);
        foreach (Collider col in colliders)
        {
            float shortDistance2 = Vector3.Distance(transform.position, col.transform.position);
            if (shortDistance > shortDistance2)
            {
                if (CorrectAirGround(col.gameObject) == true) 
                {
                    shortDistance = shortDistance2;
                    shortestEnemy = col;
                }

            }
        }
        if (CorrectAirGround(shortestEnemy.gameObject) == false) { return null; }
        return shortestEnemy.gameObject;
    }

    public float SetAttackStoppingDistance() //공격시 정지거리 설정 
    {
        return (transform.lossyScale.x + transform.lossyScale.z) / 4 + unitBaseData.attackRange + 0.2f;
    }
    #endregion
    #region Gathering
    public IEnumerator GatheringCoroutine(GameObject targetMaterial) //자원 채취(자원 클릭)
    {
        if(!targetMaterial.GetComponent<MaterialManager>())// 자원 이외 대상 클릭 시
        {
            Debug.Log("incorrect target");
            StopMove();
            yield break; 
        }
        objectState = ObjectState.Gathering;
        m_NavMestAgent.avoidancePriority = 60;
        m_NavMestAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        m_NavMestAgent.stoppingDistance = 0;
        bool bringMaterial = false;
        float Timer = 0f;
        GameObject nowTargetMaterial = targetMaterial;
        targetObject = targetMaterial;
        MaterialManager targetMaterialComponent = targetObject.GetComponent<MaterialManager>();
        HoldingMaterialManager holdingMaterialManagerComponent = HoldingMaterialManager.GetComponent<HoldingMaterialManager>();
        MaterialType targetMaterialType = targetMaterialComponent.materialType;
        bool changeTarget = false;
        while(objectState == ObjectState.Gathering)
        {
            m_NavMestAgent.speed = unitBaseData.moveSpeed * Time.deltaTime;
            Timer += 0.1f;
            m_NavMestAgent.stoppingDistance = SetStoppingDistance();

            if (bringMaterial == false && holdingMaterialManagerComponent.holdingMaterialType == MaterialType.None) // 자원으로 이동
            {
                if (changeTarget == true)
                {
                    if (targetMaterial == null || targetMaterial.GetComponent<MaterialManager>().GatheringObject != null)
                    {
                        targetObject = IsAroundMaterial(targetMaterial.GetComponent<MaterialManager>(), targetMaterialType);
                        nowTargetMaterial = targetObject;
                        targetMaterialComponent = targetObject.GetComponent<MaterialManager>();
                    }
                    else
                    {
                        targetObject = targetMaterial;
                        nowTargetMaterial = targetObject;
                        targetMaterialComponent = targetObject.GetComponent<MaterialManager>();
                    }
                }
                else
                {
                    targetObject = nowTargetMaterial;
                    targetMaterialComponent = targetObject.GetComponent<MaterialManager>();
                }
                
                changeTarget = false;
                while(true)
                {
                    if(targetObject == null)
                    {
                        changeTarget = true;
                        break;
                    }
                    targetEnd = SetTargetEnd(targetObject);
                    m_NavMestAgent.SetDestination(targetEnd);
                    if (Timer >= 2f)
                    {
                        if (IsBlocked())
                        {
                            StopMove();
                            yield break;
                        }
                    }
                    if (IsArrived())  // 자원 채취
                    {
                        Debug.Log("Get Material");
                        transform.LookAt(targetObject.transform);
                        if(targetMaterialComponent.GatheringObject != null && targetMaterialComponent.GatheringObject != gameObject)
                        {
                            changeTarget = true;
                            break;
                        }
                        targetMaterialComponent.GatheringObject = gameObject;
                        yield return new WaitForSecondsRealtime(2f);

                        Debug.Log("Take Material");
                        holdingMaterialManagerComponent.GatherMaterial(targetMaterialType, targetMaterialComponent.GatheredMaterial());
                        bringMaterial = true;
                        Timer = 0f;
                        break;
                    }
                    yield return new WaitForEndOfFrame();
                }
            }
            else if(bringMaterial == false && holdingMaterialManagerComponent.holdingMaterialType != MaterialType.None)
            {
                bringMaterial = true;
            }
            else if (bringMaterial == true && holdingMaterialManagerComponent.holdingMaterialType != MaterialType.None) // 커맨드센터로 이동
            {
                Debug.Log("put Material");
                targetObject = IsAroundBuilding(BuildingName.사령부);
                targetEnd = SetTargetEnd(targetObject);
                m_NavMestAgent.SetDestination(targetEnd);

                if (Timer >= 2f)
                {
                    if (IsBlocked())
                    {
                        StopMove();
                        yield break;
                    }
                }
                if (IsArrived())
                {
                    Debug.Log("자원 반환");
                    transform.LookAt(targetObject.transform);
                    holdingMaterialManagerComponent.PutMaterial();
                    bringMaterial = false;
                    Timer = 0f;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public GameObject IsAroundBuilding(BuildingName buildingName) // 가장 가까운 건물 탐색
    {
        BuildingManager[] everyBuilding = FindObjectsOfType<BuildingManager>();
        if(everyBuilding.Length > 0)
        {
            BuildingManager shortBuilding = everyBuilding[0];
            float shortDistance = Vector3.Distance(transform.position, everyBuilding[0].transform.position);
            foreach (BuildingManager bui in everyBuilding)
            {
                if (bui.GetData().buildingName == buildingName)
                {
                    float shortDistance2 = Vector3.Distance(transform.position, bui.transform.position);
                    if (shortDistance > shortDistance2)
                    {
                        shortDistance = shortDistance2;
                        shortBuilding = bui;
                    }
                }
            }
            return shortBuilding.gameObject;
        }
        else { Debug.Log("IsAroundBuilding Error"); return null; }
    }
    public GameObject IsAroundMaterial(MaterialManager nowTarget, MaterialType materialType) // 가장 가까운 자원 탐색
    {
        MaterialManager[] everyMaterial = FindObjectsOfType<MaterialManager>();
        if (everyMaterial.Length > 0)
        {
            MaterialManager shortMaterial = everyMaterial[0];
            float shortDistance = Vector3.Distance(transform.position, everyMaterial[0].transform.position);
            foreach (MaterialManager material in everyMaterial)
            {
                if (material.materialType == materialType && material.remainMaterial > 0 && material.GatheringObject == false && material != nowTarget)
                {
                    float shortDistance2 = Vector3.Distance(transform.position, material.transform.position);
                    if (shortDistance > shortDistance2)
                    {
                        shortDistance = shortDistance2;
                        shortMaterial = material;
                    }
                }
            }
            return shortMaterial.gameObject;
        }
        else { Debug.Log("IsAroundBuilding Error"); return null; }
    }
    #endregion
    private void OnTriggerEnter(Collider collider) //다른 오브젝트와 충돌 시
    {
        if (objectState == ObjectState.Attack || objectState == ObjectState.Patrol || objectState == ObjectState.Move || objectState == ObjectState.Gathering)
        {
            //Debug.Log("OnTriggerEnter");
            isCollisionTimer = 0f;
            if (ReferenceEquals(collider.gameObject, targetObject))
            {
                isCollisionTarget = true;
            }
            else
            {
                isCollisionObject = true;
            }
        }
    }
    private void OnTriggerStay(Collider collider) //다른 오브젝트와 충돌 중
    {
        if (objectState == ObjectState.Attack || objectState == ObjectState.Patrol || objectState == ObjectState.Move || objectState == ObjectState.Gathering)
        {
            isCollisionTimer += (1 * Time.deltaTime);
        }
    }
    private void OnTriggerExit(Collider collider) //다른 오브젝트와 충돌 해제 시
    {
        if (objectState == ObjectState.Attack || objectState == ObjectState.Patrol || objectState == ObjectState.Move || objectState == ObjectState.Gathering)
        {
            //Debug.Log("OnTriggerExit");
            isCollisionTimer = 0f;
            isCollisionObject = false;
            isCollisionTarget = false;
        }
    }
    public bool IsBlocked() // 방해 여부
    {
        if (m_NavMestAgent.velocity.magnitude <= 0.1f && isCollisionTimer >= 2.5f)
        {
            Debug.Log("IsBlocked");
            isCollisionTimer = 0f;
            isCollisionObject = false;
            isCollisionTarget = false;
            return true;
        }
        return false;
    }
    public bool IsArrived() // 도착 여부
    {
        if ((m_NavMestAgent.velocity.magnitude <= 0.1f && Vector3.Distance(transform.position, targetEnd) <= m_NavMestAgent.stoppingDistance) || isCollisionTarget == true && m_NavMestAgent.velocity.magnitude <= 0.1f)
        {
            Debug.Log("Isarrived");
            isCollisionTimer = 0f;
            isCollisionObject = false;
            isCollisionTarget = false;
            return true;
        }
        return false;
    }

    public Vector3 SetTargetEnd(GameObject target) // 좌표에 따른 목표 위치 설정
    {
        Vector3 setEnd = default;
        float compareX = Mathf.Abs(transform.position.x) + Mathf.Abs(target.transform.position.x);
        float compareZ = Mathf.Abs(transform.position.z) + Mathf.Abs(target.transform.position.z);
        
        if(compareX >= compareZ)
        {
            if(transform.position.x >= target.transform.position.x)
            {
                setEnd.x = target.transform.position.x + target.transform.lossyScale.x / 2 + transform.lossyScale.x / 2;
                setEnd.z = target.transform.position.z;
            }
            else if(transform.position.x < target.transform.position.x)
            {
                setEnd.x = target.transform.position.x - target.transform.lossyScale.x / 2 - transform.lossyScale.x / 2;
                setEnd.z = target.transform.position.z;
            }
        }
        else if(compareX < compareZ)
        {
            if (transform.position.z >= target.transform.position.z)
            {
                setEnd.z = target.transform.position.z + target.transform.lossyScale.z / 2 + transform.lossyScale.z / 2;
                setEnd.x = target.transform.position.x;
            }
            else if (transform.position.z < target.transform.position.z)
            {
                setEnd.z = target.transform.position.z - target.transform.lossyScale.z / 2 - transform.lossyScale.z / 2;
                setEnd.x = target.transform.position.x;
            }
        }
        setEnd.y = 0.5f;
        return setEnd;
    }
    public float SetStoppingDistance() // 정지거리 설정
    {
        return (transform.lossyScale.x + transform.lossyScale.z) / 3;
    }

    public IEnumerator ManaRegenCoroutine()
    {
        while(true)
        {
            if (nowEnergy <= unitBaseData.maxEnergy)
            {
                nowEnergy += 1;
            }
            yield return new WaitForSeconds(1);
        }
    }
    public void SetHp()
    {
        nowHp = unitBaseData.maxHp;
    }
    public UnitBaseData GetData()
    {
        return unitBaseData;
    }
}