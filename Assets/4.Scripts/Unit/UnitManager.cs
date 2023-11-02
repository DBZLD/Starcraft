using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private GameObject Marker;
    [SerializeField] private GameObject NameText;
    [SerializeField] private UnitBaseData unitData;
    private NavMeshAgent m_NavMestAgent;
    public UnitState unitState;
    public int unitTeam;
    
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
        unitTeam = 1;
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
        m_NavMestAgent.speed = unitData.moveSpeed * Time.deltaTime;
        m_NavMestAgent.SetDestination(End);
        unitState = UnitState.Move;

        while (unitState == UnitState.Move)
        {
            if (IsStoped())
            {
                StopMove();
                yield break;
            }
            m_NavMestAgent.speed = unitData.moveSpeed * Time.deltaTime;

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
    public IEnumerator AttackCoroutine(GameObject target)
    {
        m_NavMestAgent.speed = unitData.moveSpeed * Time.deltaTime;
        m_NavMestAgent.stoppingDistance = unitData.attackRange;
        m_NavMestAgent.SetDestination(target.transform.position);

        while()
    }
    public void StopMove()
    {
        m_NavMestAgent.ResetPath();
        unitState = UnitState.Stop;
    }
    public bool IsStoped()
    {
        if(m_NavMestAgent.velocity.magnitude <= 0.5f && m_NavMestAgent.remainingDistance <= 0.1f)
        {
            return true;
        }
        return false;
    }
}