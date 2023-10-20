using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//StopMove , unitStatus Ãß°¡
public class UnitManager : MonoBehaviour
{
    [SerializeField] private GameObject Marker;
    [SerializeField] private GameObject NameText;
    [SerializeField] private UnitBaseData unitData;
    private NavMeshAgent m_NavMestAgent;
    public UnitStatus unitStatus;

    public UnitStatus unitStatus;

    private void Awake()
    {
        m_NavMestAgent = GetComponent<NavMeshAgent>();
<<<<<<< HEAD

        Marker.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        Marker.transform.localPosition = new Vector3(0, -transform.lossyScale.y/2 + 0.1f, 0);
        Marker.SetActive(false);

        NameText.transform.localPosition = new Vector3(0, transform.lossyScale.y/2, 0);
        NameText.transform.rotation = Quaternion.Euler(90, 0, 0);

=======
>>>>>>> 7965a29c2f9ee661a0f2fe814583df3da6bc5eec
        unitStatus = UnitStatus.Stop;
    }

    public void MarkedUnit()
    {
        Marker.SetActive(true);
    }

    public void UnMarkedUnit()
    {
        Marker.SetActive(false);
    }

    public void Moveto(Vector3 End)
    {
        m_NavMestAgent.speed = unitData.moveSpeed;
        m_NavMestAgent.SetDestination(End);
        unitStatus = UnitStatus.Move;
    }
<<<<<<< HEAD
=======

>>>>>>> 7965a29c2f9ee661a0f2fe814583df3da6bc5eec
    public void StopMove()
    {
        m_NavMestAgent.ResetPath();
        unitStatus = UnitStatus.Stop;
    }

    public void Attackto(Vector3 End)
    {
        m_NavMestAgent.speed = unitData.moveSpeed;
    }
}
