using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AirGround  //이동 형태
{
    Ground,    //지상
    Air,       //공중
    Hovering   //부유
};
public enum AttackType //공격 타입
{
    Normal,     //일반
    Explosive,  //폭발
    Concussive, //진동
    Spell       //마법
};
public enum ObjectSize   //크기
{
    Small,  //소형
    Medium, //중형
    Large   //대형
};
public enum ObjectType   //속성
{
    Biological,           //생체
    Mechanical,           //기계
    Robotic,              //로봇
    SpellCaster,          //무생체
    BiologicalMechanical  //생체+기계
};
public enum UnitState //유닛 상태
{
    Stop,           //정지
    Move,           //이동
    Attack,         //공격
    Hold,           //홀드
    Patrol,         //패트롤
    Gathering,      //자원 채취
    Repair,         //수리
    Building,       //건설
    Healing,        //힐
    Destroy         //파괴
};

public enum ButtonNumList //버튼 리스트
{

};
public enum UnitName
{
    SCV,
    Marine
};
public enum BuildingName
{
    CommandCenter,
    Barrack
};
public enum MaterialType
{
    None,
    Mineral,
    BespeneGas
};
