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
public enum UnitSize   //유닛 크기
{
    Small,  //소형
    Medium, //중형
    Large   //대형
};
public enum UnitType   //유닛 속성
{
    Biological,           //생체
    Mechanical,           //기계
    Robotic,              //로봇
    SpellCaster,          //무생체
    BiologicalMechanical  //생체+기계
};
public enum UnitStatus //유닛 상태
{
    Stop,           //정지
    Move,           //이동
    Attack,         //공격
    ForcedAttack,   //강제 공격
    Hold,           //홀드
    Patrol          //패트롤
};
