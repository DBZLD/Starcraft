using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "UnitBaseData", menuName = "ScirptableObjects/UnitBaseData", order = 1)]
public class UnitBaseData : ScriptableObject
{
    [Header ("BaseData")]
    public UnitName unitName; //유닛 이름

    public AirGround airGround;    //유닛 이동형식
    public AttackType attackType;  //유닛 공격형식
    public AttackAirGround attackAirGround; //유닛 공격범위
    public ObjectSize objectSize;      //유닛 크기
    public ObjectType objectType;      //유닛 타입
    public UpgradeType upgradeType;    //업그레이드 타입

    [Header ("StatData")]
    public int costMineral;     //미네랄 비용
    public int costBespeneGas;  //베스핀 비용
    public int costSupply;      //인구수 비용
    public int productionTime;  //생산 시간
    public int transportSize;   //수송 크기
    public int uiPriority;      //유닛 UI 우선순위
    public int maxHp;           //최대 체력
    public int baseDefense;     //기본 방어력
    public int baseDamage;      //기본 공격력
    public int upgradeDefense;  //업그레이드 당 방어력
    public int upgradeDamage;   //업그레이드 당 공격력
    public float attackSpeed;   //공격 속도
    public float attackRange;   //공격 사거리
    public float moveSpeed;     //이동 속도

    public int maxMp;           //최대 마나
    public float regenMp;       //마나 재생

    [Header ("IsData")]
    public bool isGathering;    //자원 채취 가능
    public bool isMagic;        //마법 사용 여부
    public bool isAttack;       //공격 가능 여부


    public KeyCodeList[] keyCodeList;
}
[Serializable] public class KeyCodeList
{
    public int[] keyCode;
    public int[] buttonCode;
}

