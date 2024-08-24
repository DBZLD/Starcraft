using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System.Linq;
using System.Threading;
using System;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private GameObject Marker;
    [SerializeField] private GameObject NameText;
    [SerializeField] private UnitBaseData unitData;
    [SerializeField] private LayerMask layerEnemy;

    private NavMeshAgent m_NavMestAgent;
    public UnityEvent gatheringMineralEvent;
    public UnityEvent gatheringBespeneGasEvent;
    public UnitState unitState;
    public Coroutine coroutineList;
    public int uiPriority;
    public bool CanAttack;
    public MaterialType materialType;

    public GameObject targetObject; //타겟 오브젝트
    public float isCollisionTimer;  //충돌 시간
    public bool isCollisionTarget;  //타겟과의 충돌여부
    public bool isCollisionObject;  //타 오브젝트와의 충돌여부
    public int nowHp;               //현재 체력
    public int nowDamage;           //현재 공격력
    public int nowDefence;          //현재 방어력
    public int nowMoveSpeed;        //현재 이동 속도

    // debug
    public float UnitSpeed = 0f;


    private void Awake()
    {
        m_NavMestAgent = GetComponent<NavMeshAgent>();

        Marker.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        Marker.transform.localPosition = new Vector3(0, -transform.lossyScale.y/2 + 0.01f, 0);
        Marker.SetActive(false);

        NameText.transform.localPosition = new Vector3(0, transform.lossyScale.y/2, 0);
        NameText.transform.rotation = Quaternion.Euler(90, 0, 0);
        
        unitState = UnitState.Stop;
        m_NavMestAgent.avoidancePriority = 30;
        CanAttack = true;
        materialType = MaterialType.None;
        isCollisionTimer = 0f;
        isCollisionTarget = false;
        isCollisionObject = false;

        SetHp(unitData.maxHp);
        SetDamage(unitData.baseDamage + unitData.upgradeDamage);
        SetDefence(unitData.baseDefense + unitData.upgradeDefense);

        if (gatheringMineralEvent == null) { gatheringMineralEvent = new UnityEvent(); }
        if (gatheringBespeneGasEvent == null) { gatheringBespeneGasEvent = new UnityEvent(); }
    }
    public void OnGatheringMineral()
    {
        gatheringMineralEvent.Invoke();
    }
    public void OnGatheringBespeneGas()
    {
        gatheringBespeneGasEvent.Invoke();
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
        unitState = UnitState.Stop;
    }
    public IEnumerator MoveCoroutine(Vector3 End) // 유닛 이동(땅 클릭)
    {
        float Timer = 0f;
        unitState = UnitState.Move;
        m_NavMestAgent.avoidancePriority = 50;
        m_NavMestAgent.stoppingDistance = 0;
        m_NavMestAgent.speed = unitData.moveSpeed * Time.deltaTime;
        UnitSpeed = m_NavMestAgent.speed;
        m_NavMestAgent.SetDestination(End);

        while (unitState == UnitState.Move)
        {  
            m_NavMestAgent.speed = unitData.moveSpeed * Time.deltaTime;
            Timer += 0.1f;

            if (Timer >= 1f)
            {
                if (IsBlocked()||IsArrived())
                {
                    StopMove();
                    yield break;
                }
            }

            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
    public IEnumerator MoveCoroutine(GameObject target) //유닛 이동(오브젝트 클릭)
    {
        unitState = UnitState.Move;
        m_NavMestAgent.avoidancePriority = 50;
        targetObject = target;
        m_NavMestAgent.stoppingDistance = 0;
        float Timer = 0f;

        while (unitState == UnitState.Move)
        {
            m_NavMestAgent.speed = unitData.moveSpeed * Time.deltaTime;
            UnitSpeed = m_NavMestAgent.speed;
            m_NavMestAgent.SetDestination(target.transform.position);
            Timer += 0.1f;

            if (Timer >= 1f)
            {
                if (IsBlocked() || IsArrived())
                {
                    StopMove();
                    yield break;
                }
            }

            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    public IEnumerator AttackCoroutine(GameObject target) //유닛 공격(오브젝트 클릭)
    {
        float Timer = 0f;
        unitState = UnitState.Attack;
        m_NavMestAgent.avoidancePriority = 50;
        targetObject = target;
        while (unitState == UnitState.Attack)
        {
            if (!unitData.isAttack) { yield break; }
            m_NavMestAgent.speed = unitData.moveSpeed * Time.deltaTime;
            m_NavMestAgent.stoppingDistance = unitData.attackRange + SetStopingDistance(target);
            m_NavMestAgent.SetDestination(target.transform.position);
            Timer += 0.1f;

            if (Timer >= 1f)
            {
                if ((target.GetComponent<UnitManager>().unitState == UnitState.Destroy) || IsBlocked())
                {
                    StopMove();
                    yield break;
                }
            }

            if(IsArrived())
            {

            }

            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    public IEnumerator AttackCoroutine(Vector3 End) //유닛 공격(땅 클릭)
    {
        unitState = UnitState.Attack;
        m_NavMestAgent.avoidancePriority = 50;
        m_NavMestAgent.stoppingDistance = 0;
        float Timer = 0f;
        while (unitState == UnitState.Attack)
        {
            m_NavMestAgent.speed = unitData.moveSpeed * Time.deltaTime;
            UnitSpeed = m_NavMestAgent.speed;
            m_NavMestAgent.SetDestination(End);
            Timer += 0.1f;

            if(Timer >= 1f)
            {
                if (IsBlocked() || IsArrived())
                {
                    StopMove();
                    yield break;
                }
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, 2f + unitData.attackRange, layerEnemy);
            if (colliders[0] != null)
            {
                yield return StartCoroutine(AttackCoroutine(ShortestEnemy(colliders).gameObject));
            }

            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    public IEnumerator HoldCoroutine() //홀드
    {
        m_NavMestAgent.ResetPath();
        m_NavMestAgent.avoidancePriority = 30;
        unitState = UnitState.Hold;
        while(unitState == UnitState.Hold)
        {
            Collider[] enemy;
            enemy = Physics.OverlapSphere(transform.position, unitData.attackRange, layerEnemy);
            if(enemy.Length <= 0) { yield break; }

            Collider arroundEnemy;
            arroundEnemy = ShortestEnemy(enemy);

            Debug.Log(arroundEnemy);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    #region Gathering
    public IEnumerator GatheringCoroutine(GameObject target) //자원 채취(자원 클릭)
    {
        unitState = UnitState.Gathering;
        m_NavMestAgent.stoppingDistance = 0;
        bool bringMaterial = false;
        float Timer = 0f;

        if(!target.GetComponent<MaterialManager>())
        {
            Debug.Log("올바른 대상이 아닙니다."); //자원 이외 대상 클릭 시
            yield break; 
        }
        MaterialManager material = target.GetComponent<MaterialManager>();

        while(unitState == UnitState.Gathering && material.remainMaterial > 0)
        {
            m_NavMestAgent.speed = unitData.moveSpeed * Time.deltaTime;
            Timer += 0.1f;
            if(bringMaterial == false && materialType == MaterialType.None)
            {
                targetObject = target;
                m_NavMestAgent.SetDestination(target.transform.position);
                if(Timer >= 1f)
                {
                    if (IsBlocked() || IsArrived())
                    {
                        material.isGathering = true;
                        yield return new WaitForSecondsRealtime(2f);
                        GatheringMaterial(material);
                        bringMaterial = true;
                    }
                }
            }
            else if(bringMaterial == false && materialType != MaterialType.None)
            {
                bringMaterial = true;
            }
            else if(bringMaterial == true && materialType != MaterialType.None)
            {
                targetObject = IsAroundBuilding(BuildingName.CommandCenter).gameObject;
                m_NavMestAgent.SetDestination(IsAroundBuilding(BuildingName.CommandCenter).transform.position);
                if(Timer >= 1f)
                {
                    if (IsBlocked() || IsArrived())
                    {
                        BringingMaterial();
                        bringMaterial = false;
                    }
                }
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
        yield break;
    }
    public void GatheringMaterial(MaterialManager target) //자원을 채취할 때 호출
    {
        if (target.materialType == MaterialType.Mineral)
        {
            target.isGathering = false;
            if(target.remainMaterial <= 8)
            {
                target.remainMaterial -= target.remainMaterial;
            }
            else
            {
                target.remainMaterial -= 8;

            }
            target.remainMaterial -= 8;
            materialType = target.materialType;
        }
        else if (target.materialType == MaterialType.BespeneGas)
        {
            target.isGathering = false;
            target.remainMaterial -= 8;
            materialType = target.materialType;
        }
    }
    public void BringingMaterial() //자원을 갖다 놓을 때 호출
    {
        if (materialType == MaterialType.Mineral)
        {
            OnGatheringMineral();
            materialType = MaterialType.None;
        }
        else if (materialType == MaterialType.BespeneGas)
        {
            OnGatheringBespeneGas();
            materialType = MaterialType.None;
        }
    }
    public BuildingManager IsAroundBuilding(BuildingName buildingName) // 가장 가까운 건물 탐색
    {
        BuildingManager[] buildings;
        buildings = FindObjectsOfType<BuildingManager>();
        BuildingManager shortBuilding = buildings[0];
        float shortDistance = Vector3.Distance(transform.position, buildings[0].transform.position);
        foreach (BuildingManager bui in buildings)
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
        return shortBuilding;
    }
    #endregion

    public Collider ShortestEnemy(Collider[] colliders) // 가장 가까운 적 찾기
    {
        Collider shortEnemy = colliders[0];
        float shortDistance = Vector3.Distance(transform.position, colliders[0].transform.position);
        foreach (Collider col in colliders)
        {
            float shortDistance2 = Vector3.Distance(transform.position, col.transform.position);
            if (shortDistance > shortDistance2)
            {
                shortDistance = shortDistance2;
                shortEnemy = col;
            }
        }
        return shortEnemy; 
    }
    public float SetStopingDistance(GameObject target) // 정지 거리
    {
        return (target.transform.lossyScale.x + target.transform.lossyScale.y) / 1.5f;
    }
    public IEnumerator AttackCoolTimeCoroutine(GameObject target) // 공격 쿨타임
    {
        if (CanAttack == true)
        {
            CanAttack = false;
            TakeDamage(unitData.attackType);
            yield return new WaitForSecondsRealtime(unitData.attackSpeed);
        }
        else
        {
            CanAttack = true;
        }
    }
    public void TakeDamage(AttackType attackType)
    {

    }
    private void OnTriggerEnter(Collider collider) //다른 오브젝트와 충돌 시
    {
        if (unitState == UnitState.Attack || unitState == UnitState.Patrol || unitState == UnitState.Move || unitState == UnitState.Gathering)
        {
            Debug.Log("OnTriggerEnter");
            Debug.Log("collider" + collider);
            isCollisionTimer = 0f;
            if (targetObject == collider.gameObject)
            {
                isCollisionTarget = true;
            }
            else
            {
                isCollisionObject = true;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (unitState == UnitState.Attack || unitState == UnitState.Patrol || unitState == UnitState.Move || unitState == UnitState.Gathering)
        {
            isCollisionTimer += (1 * Time.deltaTime);
            Debug.Log("Timer" + isCollisionTimer);
        }
    }
    private void OnTriggerExit(Collider other) //다른 오브젝트와 충돌 해제 시
    {
        if (unitState == UnitState.Attack || unitState == UnitState.Patrol || unitState == UnitState.Move || unitState == UnitState.Gathering)
        {
            Debug.Log("OnTriggerExit");
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
        if (m_NavMestAgent.velocity.magnitude <= 0.1f && m_NavMestAgent.remainingDistance <= SetStopingDistance(gameObject))
        {
            Debug.Log("Isarrived");
            isCollisionTimer = 0f;
            isCollisionObject = false;
            isCollisionTarget = false;
            return true;
        }
        return false;
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
        nowHp = unitData.maxHp;
    }
    public void SetDamage()
    {
        nowDamage = unitData.baseDamage + unitData.upgradeDamage;

    }
    public void SetDefence()
    {
        nowDefence = unitData.baseDefense + unitData.upgradeDefense;

    }
    public UnitBaseData GetData()
    {
        return unitData;
    }
}