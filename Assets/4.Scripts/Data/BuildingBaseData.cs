using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingBaseData", menuName = "ScirptableObjects/BuildingBaseData", order = 1)]
public class BuildingBaseData : ScriptableObject
{
    public int costMineral;     //미네랄 가격
    public int costBespeneGas;  //가스 가격
    public int productionTime;  //생산 시간

    public float buildingMaxHp;             //체력
    public float buildingBaseDefense;       //기본 방어력

    public bool isAttack;                   //공격 여부
    public float buildingBaseDamage;        //기본 공격력
    public float buildingAttackSpeed;       //공격 속도
    public float buildingAttackRange;       //공격 사거리

    public bool isMove;             //이동 여부
    public float buildingMoveSpeed; //이동 속도
    public int buildingAirGround;   //이동 형태

}
enum BuildingAirGround  //이동 형태
{
    Ground,    //지상
    Air,       //공중
};

