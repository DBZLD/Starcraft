using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private GameObject Marker;
    [SerializeField] private GameObject NameText;
    [SerializeField] private UnitBaseData unitData;
    [SerializeField] private LayerMask layerEnemy;

    private NavMeshAgent m_NavMestAgent;
    private PlayerData m_playerData;
    public UnitState unitState;
    public Coroutine coroutineList;
    public int uiPriority;
    public bool CanAttack;

    private void Awake()
    {
        m_NavMestAgent = GetComponent<NavMeshAgent>();
        m_playerData = GetComponent<PlayerData>();

        Marker.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        Marker.transform.localPosition = new Vector3(0, -transform.lossyScale.y/2 + 0.01f, 0);
        Marker.SetActive(false);

        NameText.transform.localPosition = new Vector3(0, transform.lossyScale.y/2, 0);
        NameText.transform.rotation = Quaternion.Euler(90, 0, 0);
        
        unitState = UnitState.Stop;
        m_NavMestAgent.avoidancePriority = 50;
        CanAttack = true;

        SetHp(unitData.maxHp);
        SetDamage();
        SetDefence();
    }
    
    public void MarkedUnit()
    {
        Marker.SetActive(true);
    }

    public void UnMarkedUnit()
    {
        Marker.SetActive(false);
    }
    public void StopMove()
    {
        m_NavMestAgent.ResetPath();
        m_NavMestAgent.avoidancePriority = 50;
        unitState = UnitState.Stop;
    }
    public IEnumerator MoveCoroutine(Vector3 End)
    {
        unitState = UnitState.Move;
        m_NavMestAgent.avoidancePriority = 30;
        m_NavMestAgent.stoppingDistance = 0;

        while (unitState == UnitState.Move)
        {  
            m_NavMestAgent.speed = unitData.moveSpeed * Time.deltaTime;
            m_NavMestAgent.SetDestination(End);

            if (IsArrived())
            {
                StopMove();
                yield break;
            }

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
    public IEnumerator AttackCoroutine(GameObject target)
    {
        unitState = UnitState.Attack;
        m_NavMestAgent.avoidancePriority = 30;
        while (unitState == UnitState.Attack)
        {
            if (!unitData.isAttack) { yield break; }
            m_NavMestAgent.speed = unitData.moveSpeed * Time.deltaTime;
            m_NavMestAgent.stoppingDistance = unitData.attackRange;
            m_NavMestAgent.SetDestination(target.transform.position);

            if ((target.GetComponent<UnitManager>().unitState == UnitState.Destroy))
            {
                StopMove();
                yield break;
            }
            if(IsArrived())
            {
                Debug.Log("arrived");
            }

            yield return new WaitForSecondsRealtime(0.5f);
        } 
    }
    public IEnumerator AttackCoroutine(Vector3 End)
    {
        unitState = UnitState.Attack;
        m_NavMestAgent.avoidancePriority = 30;
        m_NavMestAgent.stoppingDistance = 0;

        while (unitState == UnitState.Attack)
        {
            m_NavMestAgent.speed = unitData.moveSpeed * Time.deltaTime;
            m_NavMestAgent.SetDestination(End);

            if (IsArrived())
            {
                StopMove();
                yield break;
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, 7f, layerEnemy); ;
            if (colliders.Length > 0)
            {
                Debug.Log("find enemy");
                yield return StartCoroutine(AttackCoroutine(ShortEnemy(colliders).gameObject));
            }

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
    public IEnumerator HoldCoroutine()
    {
        m_NavMestAgent.ResetPath();
        m_NavMestAgent.avoidancePriority = 50;
        unitState = UnitState.Hold;
        while(unitState == UnitState.Hold)
        {
            Collider[] enemy;
            enemy = Physics.OverlapSphere(transform.position, unitData.attackRange, layerEnemy);
            if(enemy.Length <= 0) { yield break; }

            Collider arroundEnemy;
            arroundEnemy = ShortEnemy(enemy);

            Debug.Log(arroundEnemy);
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
    #region Gathering
    public IEnumerator GatheringCoroutine(GameObject target)
    {
        unitState = UnitState.Gathering;
        m_NavMestAgent.stoppingDistance = 0;
        bool bringMaterial = false;
        if(!target.GetComponent<MaterialManager>()) { yield break; }
        MaterialManager material = target.GetComponent<MaterialManager>();

        while(unitState == UnitState.Gathering && material.remainMaterial > 0)
        {
            m_NavMestAgent.speed = unitData.moveSpeed * Time.deltaTime;

            if(bringMaterial == false && unitData.materialType == MaterialType.None)
            {
                m_NavMestAgent.SetDestination(target.transform.position);
                if (IsArrived())
                {
                    yield return new WaitForSecondsRealtime(2f);
                    GatheringMaterial(material);
                    bringMaterial = true;
                }
            }
            else if(bringMaterial == true && unitData.materialType != MaterialType.None)
            {
                m_NavMestAgent.SetDestination(IsAroundBuilding(BuildingName.CommandCenter).transform.position);
                if (IsArrived()) 
                {
                    BringingMaterial(m_playerData);
                    bringMaterial = false;
                }
            }
            yield return new WaitForSecondsRealtime(0.5f);
        }
        yield break;
    }
    public BuildingManager IsAroundBuilding(BuildingName buildingName)
    {
        BuildingManager[] buildings;
        buildings = FindObjectsOfType<BuildingManager>();
        BuildingManager shortBuilding = buildings[0];
        float shortDistance = Vector3.Distance(transform.position, buildings[0].transform.position);
        foreach (BuildingManager bui in buildings)
        {
            if (bui.buildingData.buildingName == buildingName)
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
    public void GatheringMaterial(MaterialManager target)
    {
        if (target.isGathering == false)
        {
            if (target.materialType == MaterialType.Mineral)
            {
                target.isGathering = true;
                target.remainMaterial -= 8;
                unitData.materialType = target.materialType;
            }
            else if (target.materialType == MaterialType.BespeneGas)
            {
                target.isGathering = true;
                target.remainMaterial -= 8;
                unitData.materialType = target.materialType;
            }
        }
    }
    public void BringingMaterial(PlayerData player)
    {
        if (unitData.materialType == MaterialType.Mineral)
        {
            player.mineral += 8;
            unitData.materialType = MaterialType.None;
        }
        else if (unitData.materialType == MaterialType.BespeneGas)
        {
            player.bespeneGas += 8;
            unitData.materialType = MaterialType.None;
        }
    }
    #endregion
    public bool IsArrived()
    {
        if(m_NavMestAgent.velocity.magnitude >= 0.5f && m_NavMestAgent.remainingDistance <= gameObject.transform.lossyScale.x/2*3) { return true; }
        return false;
    }
    public Collider ShortEnemy(Collider[] colliders)
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
    public IEnumerator AttackCoolTimeCoroutine(GameObject target)
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
    public void SetHp(int hp)
    {
        unitData.nowHp = hp;
    }
    public void SetDamage()
    {
        unitData.nowDamage = unitData.baseDamage + unitData.upgradeDamage;

    }
    public void SetDefence()
    {
        unitData.nowDefense = unitData.baseDefense + unitData.upgradeDefense;

    }
    public UnitBaseData GetData()
    {
        return unitData;
    }
}