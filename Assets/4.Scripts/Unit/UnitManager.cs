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
    public UnitStatus unitStatus;
    public int unitTeam;

    public int uiPriority;

    UnitName unitName;
    AirGround airGround;

    int maxHp = 1;          
    int Damage = 0;            
    int Defence = 0;

    float attackSpeed = 0f;      
    float attackRange = 0f;      
    float moveSpeed = 0f;        

    bool isMagic;       
    bool isAttack;      

    float maxMp = 0;      

    private void Awake()
    {
        m_NavMestAgent = GetComponent<NavMeshAgent>();

        Marker.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        Marker.transform.localPosition = new Vector3(0, -transform.lossyScale.y/2 + 0.01f, 0);
        Marker.SetActive(false);

        NameText.transform.localPosition = new Vector3(0, transform.lossyScale.y/2, 0);
        NameText.transform.rotation = Quaternion.Euler(90, 0, 0);

        unitStatus = UnitStatus.Stop;
        SetUnitStatus();
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
    public void Moveto(Vector3 End)
    {
        m_NavMestAgent.SetDestination(End);
        unitStatus = UnitStatus.Move;
    }

    public void StopMove()
    {
        m_NavMestAgent.ResetPath();
        unitStatus = UnitStatus.Stop;
    }

    public void SetUnitStatus()
    {
        unitName = unitData.unitName;
        airGround = unitData.airGround;
        maxHp = unitData.maxHp;

        if (unitData.isAttack) { Damage = unitData.baseDamage; }
        Defence = unitData.baseDefense;

        attackSpeed = unitData.attackSpeed;
        attackRange = unitData.attackRange;
        moveSpeed = unitData.moveSpeed;

        if(unitData.isMagic) { maxMp = unitData.maxMp; }
    }
}