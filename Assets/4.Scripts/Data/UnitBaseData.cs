using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "UnitBaseData", menuName = "ScirptableObjects/UnitBaseData", order = 1)]
public class UnitBaseData : ScriptableObject
{
    public UnitName unitName; //유닛 이름

    public AirGround airGround;    //이동 형태
    public AttackType attackType;  //공격 타입
    public UnitSize unitSize;      //유닛 크기
    public UnitType unitType;      //유닛 속성

    public int costMineral;     //미네랄 가격
    public int costBespeneGas;  //가스 가격
    public int costSupply;      //인구수
    public int productionTime;  //생산 시간
    public int transportSize;   //수송 시 크기
    public int uiPriority;      //버튼 UI 우선순위
    public int maxHp;           //체력
    public int baseDefense;     //기본 방어력
    public int baseDamage;      //기본 공격력
    public int upgradeDefense;  //업그레이드 당 방어력
    public int upgradeDamage;   //업그레이드 당 공격력
    public float attackSpeed;   //공격 속도
    public float attackRange;   //공격 사거리
    public float moveSpeed;     //이동 속도

    public bool isMagic;        //마법 사용
    public bool isAttack;       //공격 여부
    public bool isMeterial;     //자원 수송

    public int maxMp;           //마나
    public float regenMp;       //마나 재생

    public KeyCodeList[] keyCodeList;
}
[Serializable]
public class KeyCodeList
{
    public int[] keyCode;
    public int[] buttonCode;
}

