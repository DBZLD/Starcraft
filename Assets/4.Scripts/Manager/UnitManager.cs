using System.Collections;
using UnityEngine;
using UnityEngine.AI;
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

    [Header ("State")]
    public ObjectState objectState;
    public int uiPriority;
    public bool canAttack;

    [Header ("Collision")]
    public GameObject targetObject; //타겟 오브젝트
    public Vector3 targetEnd;       //타겟 좌표
    public float isCollisionTimer;  //충돌 시간
    public bool isCollisionTarget;  //타겟과의 충돌여부
    public bool isCollisionObject;  //타 오브젝트와의 충돌여부     

    [Header ("Status")]
    public int nowHp;               //현재 체력
    public int nowDamage;           //현재 공격력
    public int nowDefence;          //현재 방어력
    public int nowMoveSpeed;        //현재 이동 속도

    private NavMeshAgent m_NavMestAgent;
    // debug
    public float UnitSpeed = 0f;

    private void Awake()
    {
        m_NavMestAgent = GetComponent<NavMeshAgent>();

        Marker.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        Marker.transform.localPosition = new Vector3(0, -transform.localScale.y/2 + 0.01f, 0);
        Marker.SetActive(false);

        NameText.transform.localPosition = new Vector3(0, transform.localScale.y/2, 0);
        NameText.transform.rotation = Quaternion.Euler(90, 0, 0);
        
        objectState = ObjectState.Stop;
        m_NavMestAgent.avoidancePriority = 30;
        canAttack = true;
        isCollisionTimer = 0f;
        isCollisionTarget = false;
        isCollisionObject = false;

        SetHp(unitBaseData.maxHp);
        SetDamage(unitBaseData.baseDamage);
        SetDefence(unitBaseData.baseDefense);

         if (unitBaseData.isAttack == true) { StartCoroutine(AttackCoolTimeCoroutine()); } 
    }
    public void MarkedUnit() // 유닛 선택 시 마크 표시
    {
        Marker.SetActive(true);
    }
    public void UnMarkedUnit() // 유닛 선택 해제 시 마크 비표시
    {
        Marker.SetActive(false);
    }
    public void StopMove() // 정지
    {
        m_NavMestAgent.ResetPath();
        m_NavMestAgent.avoidancePriority = 30;
        targetObject = default;
        targetEnd = default;
        
        objectState = ObjectState.Stop;
    }
    public IEnumerator MoveCoroutine(Vector3 End) // 유닛 이동(땅 클릭)
    {
        float Timer = 0f;
        objectState = ObjectState.Move;
        m_NavMestAgent.avoidancePriority = 50;
        m_NavMestAgent.stoppingDistance = 0;
        targetEnd = End;

        while (objectState == ObjectState.Move)
        {  
            m_NavMestAgent.speed = unitBaseData.moveSpeed * Time.deltaTime;
            m_NavMestAgent.stoppingDistance = SetStoppingDistance();
            m_NavMestAgent.SetDestination(End);
            UnitSpeed = m_NavMestAgent.velocity.magnitude;
            Debug.Log("타겟과의 거리 :" + Vector3.Distance(transform.position, targetEnd) + "정지 거리" + m_NavMestAgent.stoppingDistance);
            Timer += 0.1f;

            if (Timer >= 1f)
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
        objectState = ObjectState.Move;
        m_NavMestAgent.avoidancePriority = 50;
        targetObject = target;
        targetEnd = targetObject.transform.position;
        m_NavMestAgent.stoppingDistance = 0;
        float Timer = 0f;

        while (objectState == ObjectState.Move)
        {
            m_NavMestAgent.speed = unitBaseData.moveSpeed * Time.deltaTime;
            m_NavMestAgent.stoppingDistance = SetStoppingDistance();
            UnitSpeed = UnitSpeed = m_NavMestAgent.velocity.magnitude;
            m_NavMestAgent.SetDestination(target.transform.position + SetEnd(target));
            UnitSpeed = m_NavMestAgent.velocity.magnitude;
            Debug.Log("타겟과의 거리 :" + Vector3.Distance(transform.position, targetEnd) + "정지 거리" + m_NavMestAgent.stoppingDistance);
            Timer += 0.1f;

            if (Timer >= 1f)
            {
                if (IsBlocked() || IsArrived())
                {
                    StopMove();
                    yield break;
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
    #region attack
     public IEnumerator AttackCoroutine(Vector3 End) //유닛 공격(땅 클릭)
    {
        objectState = ObjectState.Attack;
        m_NavMestAgent.avoidancePriority = 50;
        m_NavMestAgent.stoppingDistance = 0;
        float Timer = 0f;

        while (objectState == ObjectState.Attack)
        {
            m_NavMestAgent.speed = unitBaseData.moveSpeed * Time.deltaTime;
            Timer += 0.1f;
            GameObject targetEnemy = null;
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2f + unitBaseData.attackRange, layerEnemy);

            if (colliders.Length > 0 || targetEnemy != null)
            {
                targetEnemy = ShortestEnemy(colliders);
                EnemyManager targetEnemyComponent = targetEnemy.GetComponent<EnemyManager>();

                m_NavMestAgent.stoppingDistance = SetAttackStoppingDistance();
                m_NavMestAgent.SetDestination(targetEnemy.transform.position + SetEnd(targetEnemy));
                UnitSpeed = m_NavMestAgent.velocity.magnitude;
                Debug.Log("타겟과의 거리 :" + Vector3.Distance(transform.position, targetEnd) + "정지 거리" + m_NavMestAgent.stoppingDistance);
                if (Timer >= 1f)
                {
                    if ((targetEnemy.GetComponent<EnemyManager>().objectState == ObjectState.Destroy) || IsBlocked())
                    {
                        StopMove();
                        yield break;
                    }
                }
                if(IsArrived())
                {
                    transform.LookAt(targetEnemy.transform);
                    if (canAttack == true)
                    {
                        targetEnemyComponent.TakeDamage(nowDamage, unitBaseData.attackType);
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
                targetEnd = End;
                m_NavMestAgent.stoppingDistance = SetStoppingDistance();
                m_NavMestAgent.SetDestination(End);
                UnitSpeed = m_NavMestAgent.velocity.magnitude;
                Debug.Log("타겟과의 거리 :" + Vector3.Distance(transform.position, targetEnd) + "정지 거리" + m_NavMestAgent.stoppingDistance);

                if (Timer >= 1f)
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
        m_NavMestAgent.avoidancePriority = 50;
        targetObject = targetEnemy;
        targetEnd = targetObject.transform.position;
        if (!targetEnemy.CompareTag("Enemy")) { yield break; }
        EnemyManager targetEnemyComponent = targetEnemy.GetComponent<EnemyManager>();
        while (objectState == ObjectState.Attack)
        {
            if (!unitBaseData.isAttack) { yield break; }
            m_NavMestAgent.speed = unitBaseData.moveSpeed * Time.deltaTime;
            m_NavMestAgent.stoppingDistance = SetAttackStoppingDistance();
            m_NavMestAgent.SetDestination(targetEnemy.transform.position + SetEnd(targetEnemy));
            UnitSpeed = m_NavMestAgent.velocity.magnitude;
            Debug.Log("타겟과의 거리 :" + Vector3.Distance(transform.position, targetEnd) + "정지 거리" + m_NavMestAgent.stoppingDistance);
            Timer += 0.1f;

            if (Timer >= 1f)
            {
                if ((targetEnemyComponent.objectState == ObjectState.Destroy) || IsBlocked())
                {
                    StopMove();
                    yield break;
                }
            }
            if (IsArrived())
            {
                transform.LookAt(targetEnemy.transform);
                if (canAttack == true)
                {
                    targetEnemyComponent.TakeDamage(nowDamage, unitBaseData.attackType);
                    canAttack = false;
                }

                else if (canAttack == false)
                {
                    Debug.Log("AttackCoolTime");
                }
            }
        }
            yield return new WaitForEndOfFrame();
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
    public void TakeDamage(int nowDamage, AttackType attackType)
    {
        nowHp -= Mathf.RoundToInt((nowDamage - nowDefence) * AttackTypeUnitSize(attackType, unitBaseData.objectSize));
    } // (기본공격력 + 업그레이드 공격력*업그레이드 횟수 ) - ( 쉴드 잔량 + 쉴드 총 방어력 ) - 총 방어력 } * 공격/방어 방식에 따른 비율 

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

        if (target.GetComponent<UnitManager>().GetData().airGround == AirGround.Air)
        {

            if(unitBaseData.attackAirGround == AttackAirGround.Air) {  return true; }
            else if(unitBaseData.attackAirGround == AttackAirGround.Ground) { return false; }
        }
        else if(target.GetComponent<UnitManager>().GetData().airGround == AirGround.Ground)
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
                shortDistance = shortDistance2;
                shortestEnemy = col;
            }
        }
        return shortestEnemy.gameObject;
    }

    public float SetAttackStoppingDistance() //공격시 정지거리 설정 
    {
        return transform.lossyScale.x / 2 + transform.lossyScale.z / 2 + unitBaseData.attackRange;
    }
    #endregion
    public IEnumerator HoldCoroutine() //홀드
    {
        m_NavMestAgent.ResetPath();
        m_NavMestAgent.avoidancePriority = 30;
        objectState = ObjectState.Hold;
        while(objectState == ObjectState.Hold)
        {
            Collider[] arroundEnemy = Physics.OverlapSphere(transform.position, unitBaseData.attackRange+transform.lossyScale.x, layerEnemy) ;
            if(arroundEnemy.Length > 0)
            {
                GameObject targetEnemy;
                targetEnemy = ShortestEnemy(arroundEnemy);
                transform.LookAt(targetEnemy.transform);
                if (canAttack == true)
                {
                    targetEnemy.GetComponent<EnemyManager>().TakeDamage(nowDamage, unitBaseData.attackType);
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
    #region Gathering
    public IEnumerator GatheringCoroutine(GameObject targetMaterial) //자원 채취(자원 클릭)
    {
        objectState = ObjectState.Gathering;
        m_NavMestAgent.stoppingDistance = 0;
        bool bringMaterial = false;
        float Timer = 0f;

        if(!targetMaterial.GetComponent<MaterialManager>())
        {
            Debug.Log("incorrect target"); //자원 이외 대상 클릭 시
            yield break; 
        }
        MaterialManager targetMaterialComponent = targetMaterial.GetComponent<MaterialManager>();

        while(objectState == ObjectState.Gathering && targetMaterialComponent.remainMaterial > 0)
        {
            m_NavMestAgent.speed = unitBaseData.moveSpeed * Time.deltaTime;
            Timer += 0.1f;
            if(bringMaterial == false && HoldingMaterialManager.GetComponent<HoldingMaterialManager>().holdingMaterialType == MaterialType.None)
            {
                //Debug.Log("For Material");
                targetObject = targetMaterial;
                targetEnd = targetObject.transform.position;
                m_NavMestAgent.stoppingDistance = SetStoppingDistance();
                m_NavMestAgent.SetDestination(targetMaterial.transform.position + SetEnd(targetMaterial));
                UnitSpeed = m_NavMestAgent.velocity.magnitude;
                Debug.Log("타겟과의 거리 :" + Vector3.Distance(transform.position, targetEnd) + "정지 거리" + m_NavMestAgent.stoppingDistance);
                if (Timer >= 1f)
                {
                    if (IsBlocked())
                    {
                        StopMove();
                        yield break;
                    }
                }
                if (IsArrived())
                {
                    Debug.Log("Get Material");
                    targetMaterialComponent.isGathering = true;
                    yield return new WaitForSecondsRealtime(2f);

                    Debug.Log("Take Material");
                    HoldingMaterialManager.GetComponent<HoldingMaterialManager>().GatherMaterial(targetMaterialComponent.materialType, targetMaterialComponent.GatheredMaterial());
                    bringMaterial = true;
                    Timer = 0f;
                }
                
            }
            else if(bringMaterial == false && HoldingMaterialManager.GetComponent<HoldingMaterialManager>().holdingMaterialType != MaterialType.None)
            {
                Debug.Log("Set Material");
                bringMaterial = true;
            }
            else if(bringMaterial == true && HoldingMaterialManager.GetComponent<HoldingMaterialManager>().holdingMaterialType != MaterialType.None)
            {
                Debug.Log("put Material");
                targetObject = IsAroundBuilding(BuildingName.CommandCenter);
                targetEnd = targetObject.transform.position;
                m_NavMestAgent.stoppingDistance = SetStoppingDistance();
                m_NavMestAgent.SetDestination(targetObject.transform.position + SetEnd(targetObject));
                UnitSpeed = m_NavMestAgent.velocity.magnitude;
                Debug.Log("타겟과의 거리 :" + Vector3.Distance(transform.position, targetEnd) + "정지 거리" + m_NavMestAgent.stoppingDistance);
                if (Timer >= 1f)
                {
                    if (IsBlocked())
                    {
                        StopMove();
                        yield break;
                    }
                }
                if (IsArrived())
                {
                    HoldingMaterialManager.GetComponent<HoldingMaterialManager>().PutMaterial();
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
        else { Debug.Log("IsArroundBuilding Error"); return null; }
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
    private void OnTriggerStay(Collider other) //다른 오브젝트와 충돌 중
    {
        if (objectState == ObjectState.Attack || objectState == ObjectState.Patrol || objectState == ObjectState.Move || objectState == ObjectState.Gathering)
        {
            isCollisionTimer += (1 * Time.deltaTime);
        }
    }
    private void OnTriggerExit(Collider other) //다른 오브젝트와 충돌 해제 시
    {
        if (objectState == ObjectState.Attack || objectState == ObjectState.Patrol || objectState == ObjectState.Move || objectState == ObjectState.Gathering)
        {
            //Debug.Log("OnTriggerExit");
            isCollisionTimer = 0f;
            isCollisionObject = false;
            isCollisionTarget = false;
        }
    }
    public bool IsBlocked()
    {
        if (m_NavMestAgent.velocity.magnitude <= 0.1f && isCollisionTimer >= 1.5f)
        {
            Debug.Log("IsBlocked");
            isCollisionTimer = 0f;
            isCollisionObject = false;
            isCollisionTarget = false;
            return true;
        }
        return false;
    }
    public bool IsArrived()
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

    public Vector3 SetEnd(GameObject target)
    {
        Vector3 setEnd = default;
        setEnd.y = 0f;
        if(target.transform.position.x <= transform.position.x)
        {
            setEnd.x = -(target.transform.lossyScale.x / 2);
            if (target.transform.position.z <= transform.position.z)
            {
                setEnd.z = -(target.transform.lossyScale.z / 2);
            }
            else if(target.transform.position.z > transform.position.z)
            {
                setEnd.z = target.transform.lossyScale.z / 2;
            }
        }
        else if(target.transform.position.x > transform.position.x)
        {
            setEnd.x = -(target.transform.lossyScale.x / 2);
            if (target.transform.position.z <= transform.position.z)
            {
                setEnd.z = -(target.transform.lossyScale.z / 2);
            }
            else if (target.transform.position.z > transform.position.z)
            {
                setEnd.z = target.transform.lossyScale.z / 2;
            }
        }
        return setEnd;
    }
    public float SetStoppingDistance() // 정지거리 설정
    {
        return transform.lossyScale.x / 2 + transform.lossyScale.z / 2;
    }
    public void SetHp(int hp)
    {
        nowHp = hp;
    }
    public void SetDamage(int damage)
    {
        nowDamage = damage;

    }
    public void SetDefence(int defence)
    {
        nowDefence = defence;

    }
    public void SetHp()
    {
        nowHp = unitBaseData.maxHp;
    }
    public void SetDamage()
    {
        nowDamage = unitBaseData.baseDamage + unitBaseData.upgradeDamage;

    }
    public void SetDefence()
    {
        nowDefence = unitBaseData.baseDefense + unitBaseData.upgradeDefense;

    }
    public UnitBaseData GetData()
    {
        return unitBaseData;
    }
}