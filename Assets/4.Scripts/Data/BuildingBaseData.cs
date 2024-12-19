using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BuildingBaseData", menuName = "ScirptableObjects/BuildingBaseData", order = 1)]
public class BuildingBaseData : ScriptableObject
{
    [Header("BaseData")]
    public BuildingName buildingName;           //건물 이름
    public ObjectSize objectSize;               //건물 크기
    public ObjectType objectType;               //건물 타입
    public AirGround airGround;                 //건물 이동형식
    public AttackAirGround attackAirGround;     //건물 공격범위

    [Header("StatData")]
    public int costMineral;                     //소요 미네랄
    public int costVespeneGas;                  //소요 베스핀 가스
    public int buildingTime;                    //건설 시간
    public float buildingMaxHp;                 //체력
    public float buildingBaseDefense;           //기본 방어력
    public float buildingBaseDamage;            //기본 공격력
    public float buildingAttackSpeed;           //공격 속도
    public float buildingAttackRange;           //공격 사거리

    [Header("IsData")]
    public bool isAttack;                       //공격 여부

    public ButtonPageNum mainButtonPage; 
}
