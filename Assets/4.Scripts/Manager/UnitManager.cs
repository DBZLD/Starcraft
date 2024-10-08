using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System.Linq;
using System.Threading;
using System;
using Unity.VisualScripting;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private GameObject Marker;
    [SerializeField] private GameObject NameText;
    [SerializeField] private UnitBaseData unitBaseData;
    [SerializeField] private LayerMask layerEnemy;

    private NavMeshAgent m_NavMestAgent;
    public UnityEvent gatheringMineralEvent;
    public UnityEvent gatheringBespeneGasEvent;
    public Coroutine coroutineList;

    public ObjectState objectState;
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
        
        objectState = ObjectState.Stop;
        m_NavMestAgent.avoidancePriority = 30;
        CanAttack = true;
        materialType = MaterialType.None;
        isCollisionTimer = 0f;
        isCollisionTarget = false;
        isCollisionObject = false;

        SetHp(unitBaseData.maxHp);
        SetDamage(unitBaseData.baseDamage + unitBaseData.upgradeDamage);
        SetDefence(unitBaseData.baseDefense + unitBaseData.upgradeDefense);

         if (unitBaseData.isAttack == true) { StartCoroutine(AttackCoolTimeCoroutine()); } 

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
        objectState = ObjectState.Stop;
    }
    public IEnumerator MoveCoroutine(Vector3 End) // 유닛 이동(땅 클릭)
    {
        float Timer = 0f;
        objectState = ObjectState.Move;
        m_NavMestAgent.avoidancePriority = 50;
        m_NavMestAgent.stoppingDistance = 0;
        m_NavMestAgent.speed = unitBaseData.moveSpeed * Time.deltaTime;
        UnitSpeed = m_NavMestAgent.speed;
        m_NavMestAgent.SetDestination(End);

        while (objectState == ObjectState.Move)
        {  
            m_NavMestAgent.speed = unitBaseData.moveSpeed * Time.deltaTime;
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
        m_NavMestAgent.stoppingDistance = 0;
        float Timer = 0f;

        while (objectState == ObjectState.Move)
        {
            m_NavMestAgent.speed = unitBaseData.moveSpeed * Time.deltaTime;
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
            UnitSpeed = m_NavMestAgent.speed;
            Timer += 0.1f;
            GameObject target = default;

            Collider[] colliders = Physics.OverlapSphere(transform.position, 2f + unitBaseData.attackRange, layerEnemy);
            if (colliders[0] != null || target != null)
            {
                target = ShortestEnemy(colliders);
                colliders = default;
            }
            else
            {
                m_NavMestAgent.SetDestination(End); // 수정

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
    public IEnumerator AttackCoroutine(GameObject target) //유닛 공격(오브젝트 클릭)
    {
        float Timer = 0f;
        objectState = ObjectState.Attack;
        m_NavMestAgent.avoidancePriority = 50;
        targetObject = target;
        if (!target.CompareTag("Enemy")) { Debug.Log("Attack Target Error");  yield break; }

        while (objectState == ObjectState.Attack)
        {
            if (!unitBaseData.isAttack) { yield break; }
            m_NavMestAgent.speed = unitBaseData.moveSpeed * Time.deltaTime;
            m_NavMestAgent.stoppingDistance = unitBaseData.attackRange + SetStopingDistance(target);
            m_NavMestAgent.SetDestination(target.transform.position);
            Timer += 0.1f;

            if (Timer >= 1f)
            {
                if ((target.GetComponent<EnemyManager>().objectState == ObjectState.Destroy) || IsBlocked())
                {
                    StopMove();
                    yield break;
                }
            }
            if(IsArrived())
            {
                if (CanAttack == true)
                {
                    EnemyManager targetComponent = target.GetComponent<EnemyManager>();
                    TakeDamage(targetComponent.nowHp, targetComponent.nowDamage, targetComponent.GetData().airGround, targetComponent.GetData().objectSize);
                }                }
                else if(CanAttack == false)
                {
                    Debug.Log("AttackCoolTime");
                }
            }
            yield return new WaitForEndOfFrame();
    }

    public IEnumerator AttackCoolTimeCoroutine() // 공격 쿨타임
    {
        while (true)
        {
            if (CanAttack == false)
            {
                CanAttack = true;
            }
            yield return new WaitForSecondsRealtime(unitBaseData.attackSpeed);
        }
    }
    public void TakeDamage(int nowHp, int nowDefence, AirGround airGround, ObjectSize unitSize)
    {
        nowHp -= Mathf.RoundToInt((nowDamage - nowDefence) * AttackTypeUnitSize(unitBaseData.attackType, unitSize));
    } // (기본공격력 + 업그레이드 공격력*업그레이드 횟수 ) - ( 쉴드 잔량 + 쉴드 총 방어력 ) - 총 방어력 } * 공격/방어 방식에 따른 비율 

    public float AttackTypeUnitSize(AttackType attackType, ObjectSize unitSize)
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
    #endregion
    public IEnumerator HoldCoroutine() //홀드
    {
        m_NavMestAgent.ResetPath();
        m_NavMestAgent.avoidancePriority = 30;
        objectState = ObjectState.Hold;
        while(objectState == ObjectState.Hold)
        {
            Collider[] enemy;
            enemy = Physics.OverlapSphere(transform.position, unitBaseData.attackRange, layerEnemy);
            if(enemy.Length <= 0) { yield break; }

            GameObject arroundEnemy;
            arroundEnemy = ShortestEnemy(enemy);

            yield return new WaitForEndOfFrame();
        }
    }
    #region Gathering
    public IEnumerator GatheringCoroutine(GameObject target) //자원 채취(자원 클릭)
    {
        objectState = ObjectState.Gathering;
        m_NavMestAgent.stoppingDistance = 0;
        bool bringMaterial = false;
        float Timer = 0f;

        if(!target.GetComponent<MaterialManager>())
        {
            Debug.Log("올바른 대상이 아닙니다."); //자원 이외 대상 클릭 시
            yield break; 
        }
        MaterialManager material = target.GetComponent<MaterialManager>();

        while(objectState == ObjectState.Gathering && material.remainMaterial > 0)
        {
            m_NavMestAgent.speed = unitBaseData.moveSpeed * Time.deltaTime;
            Timer += 0.1f;
            if(bringMaterial == false && materialType == MaterialType.None)
            {
                targetObject = target;
                m_NavMestAgent.SetDestination(target.transform.position);
                if(Timer >= 1f)
                {
                    if (IsArrived())
                    {
                        material.isGathering = true;
                        yield return new WaitForSecondsRealtime(2f);
                        GatheringMaterial(material);
                        bringMaterial = true;
                    }
                    if(IsBlocked())
                    {
                        StopMove();
                        yield break;
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
                m_NavMestAgent.SetDestination(targetObject.transform.position);
                if(Timer >= 1f)
                {
                    if (IsBlocked() || IsArrived())
                    {
                        BringingMaterial();
                        bringMaterial = false;
                    }
                }
            }
            yield return new WaitForEndOfFrame();
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

    public GameObject ShortestEnemy(Collider[] colliders) // 가장 가까운 적 찾기
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
        return shortEnemy.gameObject; 
    }
    public float SetStopingDistance(GameObject target) // 정지 거리
    {
        return (target.transform.lossyScale.x + target.transform.lossyScale.y) / 1.5f;
    }
    private void OnTriggerEnter(Collider collider) //다른 오브젝트와 충돌 시
    {
        if (objectState == ObjectState.Attack || objectState == ObjectState.Patrol || objectState == ObjectState.Move || objectState == ObjectState.Gathering)
        {
            Debug.Log("OnTriggerEnter");
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
        if (objectState == ObjectState.Attack || objectState == ObjectState.Patrol || objectState == ObjectState.Move || objectState == ObjectState.Gathering)
        {
            isCollisionTimer += (1 * Time.deltaTime);
        }
    }
    private void OnTriggerExit(Collider other) //다른 오브젝트와 충돌 해제 시
    {
        if (objectState == ObjectState.Attack || objectState == ObjectState.Patrol || objectState == ObjectState.Move || objectState == ObjectState.Gathering)
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