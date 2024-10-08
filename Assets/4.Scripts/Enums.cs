using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AirGround  //오브젝트 이동형식
{
    Ground,    //지상
    Air,       //공중
    Hovering   //부유
};
public enum AttackAirGround
{
    Ground,
    Air,
    AirGround
};
public enum AttackType //오브젝트 공격형식
{
    Normal,     //일반형
    Explosive,  //폭발형
    Concussive, //진동형
    Spell       //마법(고정)
};
public enum ObjectSize   //오브젝트 크기
{
    Small,  //소형
    Medium, //중형
    Large   //대형
};
public enum ObjectType   //오브젝트 타입
{
    Biological,           //생체
    Mechanical,           //기계
    Robotic,              //무생체
    BiologicalMechanical  //생체+기계
};
public enum ObjectState //오브젝트 상태
{
    Stop,           //정지
    Move,           //이동
    Attack,         //공격
    Hold,           //홀드
    Patrol,         //패트롤
    Gathering,      //자원 채취
    Building,       //건설
    Destroy         //파괴
};
public enum MaterialType
{
    None,
    Mineral,
    BespeneGas
};

public enum ButtonNumList //버튼리스트
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