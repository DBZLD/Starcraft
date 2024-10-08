using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BuildingBaseData", menuName = "ScirptableObjects/BuildingBaseData", order = 1)]
public class BuildingBaseData : ScriptableObject
{
    public BuildingName buildingName;// 이름

    public ObjectSize objectSize;
    public ObjectType objectType;
    public AirGround airGround;

    public int costMineral;     //소요 미네랑
    public int costBespeneGas;  //소요 베스핀 가스
    public int productionTime;  //생산 시간

    public float buildingMaxHp;             //체력
    public float buildingBaseDefense;       //기본 방어력

    public bool isAttack;                   //공격 여부
    public float buildingBaseDamage;        //기본 공격력
    public float buildingAttackSpeed;       //공격 속도
    public float buildingAttackRange;       //공격 사거리

    public KeyCodeList[] keyCodeList;
}
