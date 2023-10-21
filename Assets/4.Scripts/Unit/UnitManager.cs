using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private GameObject Marker;
    [SerializeField] private GameObject NameText;
    [SerializeField] private UnitBaseData unitData;
    [SerializeField] private T unit;
    private NavMeshAgent m_NavMestAgent;
    public UnitStatus unitStatus;
    public int unitTeam;

    public string unitName;

    AirGround airGround;   //이동 형태

    int maxHp;             //체력
    int Damage;            //공격력
    int Defence;           //방어력
    float attackSpeed;       //공격 속도
    float attackRange;       //공격 사거리
    float moveSpeed;         //이동 속도

    bool isMagic;        //마법 사용
    bool isAttack;       //공격 여부

    float maxMp;         //마나
    float regenMp;       //마나 재생

    private void Awake()
    {
        m_NavMestAgent = GetComponent<NavMeshAgent>();

        Marker.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        Marker.transform.localPosition = new Vector3(0, -transform.lossyScale.y/2 + 0.01f, 0);
        Marker.SetActive(false);

        NameText.transform.localPosition = new Vector3(0, transform.lossyScale.y/2, 0);
        NameText.transform.rotation = Quaternion.Euler(90, 0, 0);

        unitStatus = UnitStatus.Stop;
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
        unitName = unitData.unitName; //유닛 이름

        AirGround airGround;   //이동 형태
        AttackType attackType;  //공격 타입
        UnitSize unitSize;    //유닛 크기
        UnitType unitType;    //유닛 속성

        int costMineral;     //미네랄 가격
        int costBespeneGas;  //가스 가격
        int costSupply;      //인구수
        int productionTime;  //생산 시간
        int transportSize;   //수송 시 크기

        float maxHp;             //체력
        float baseDefense;       //기본 방어력
        float baseDamage;        //기본 공격력
        float upgradeDefense;    //업그레이드 당 방어력
        float upgradeDamage;     //업그레이드 당 공격력
        float attackSpeed;       //공격 속도
        float attackRange;       //공격 사거리
        float moveSpeed;         //이동 속도

        bool isMagic;        //마법 사용
        bool isAttack;       //공격 여부

        float maxMp;         //마나
        float regenMp;       //마나 재생
    }
}

internal class T
{

}