using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private GameObject Marker;
    [SerializeField] private GameObject NameText;
    [SerializeField] private UnitBaseData unitData;
    [SerializeField] private LayerMask layerEnemy;

    private NavMeshAgent m_NavMestAgent;
    public UnitState unitState;
    public Coroutine coroutineList;
    public int uiPriority;    

    private void Awake()
    {
        m_NavMestAgent = GetComponent<NavMeshAgent>();

        Marker.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        Marker.transform.localPosition = new Vector3(0, -transform.lossyScale.y/2 + 0.01f, 0);
        Marker.SetActive(false);

        NameText.transform.localPosition = new Vector3(0, transform.lossyScale.y/2, 0);
        NameText.transform.rotation = Quaternion.Euler(90, 0, 0);
        
        unitState = UnitState.Stop;
    }
    
    public void MarkedUnit()
    {
        Marker.SetActive(true);
    }

    public void UnMarkedUnit()
    {
        Marker.SetActive(false);
    }

    public IEnumerator MoveCoroutine(Vector3 End)
    {
        unitState = UnitState.Move;

        while (unitState == UnitState.Move)
        {  
            m_NavMestAgent.speed = unitData.moveSpeed * Time.deltaTime;
            m_NavMestAgent.SetDestination(End);

            if (IsStoped())
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

            yield return new WaitForSecondsRealtime(0.5f);
        } 
    }
    public IEnumerator AttackCoroutine(Vector3 End)
    {
        unitState = UnitState.Attack;
        while (unitState == UnitState.Attack)
        {
            if (!unitData.isAttack) { yield break; }
            m_NavMestAgent.speed = unitData.moveSpeed * Time.deltaTime;
            m_NavMestAgent.stoppingDistance = unitData.attackRange;
            m_NavMestAgent.SetDestination(End);

            if (IsStoped())
            {
                StopMove();
                yield break;
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, unitData.attackRange, layerEnemy); ;
            if (colliders.Length > 0)
            {
                yield return StartCoroutine(AttackCoroutine(IsArroundEnemy(colliders).gameObject));
            }

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
    public void StopMove()
    {
        m_NavMestAgent.ResetPath();
        unitState = UnitState.Stop;
    }
    public bool IsStoped()
    {
        if(m_NavMestAgent.velocity.magnitude >= 0.5f && m_NavMestAgent.remainingDistance <= gameObject.transform.lossyScale.x/2*3) { return true; }
        return false;
    }
    public Collider IsArroundEnemy(Collider[] colliders)
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
    public void seta()
    {
        
    }
}