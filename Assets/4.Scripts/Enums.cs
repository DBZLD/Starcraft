using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AirGround  //오브젝트 이동형식
{
    Ground,    //지상
    Air,       //공중
    Hovering   //부유
};
public enum AttackAirGround //오브젝트 공격 대상
{
    Ground,     //지상
    Air,        //공중    
    AirGround   //지상+공중
};
public enum AttackType //오브젝트 공격유형
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
public enum ObjectType   //오브젝트 특성
{
    Bionic,                 //생체
    Mechanic,               //기계
    BionicMechanic,         //생체+기계
    Buildidng               //건물
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
    Destroy,         //파괴
};

public enum UpgradeType //업그레이드 타입
{
    None,           //x
    GroundBio,      //지상 생체 
    GroundMech,     //지상 기계
    Air             //공중
};
public enum MaterialType
{
    None,
    Mineral,
    VespeneGas
};
#region ButtonList
public enum ButtonStructNum //버튼번호
{
    None,
    Cancle,
    Move,
    Stop,
    Attack,
    Patrol,
    Hold,
    Gathering,
    BuildAcademy,
    BuildArmory,
    BuildBarracks,
    BuildBunker,
    BuildCommnadCenter,
    BuildComsatStation,
    BuildControlTower,
    BuildCovertOps,
    BuildEngineeringBay,
    BuildFactory,
    BuildMechineShop,
    BuildMissileTurret,
    BuildNuclearSilo,
    BuildPhysicLab,
    BuildRefinery,
    BuildScienceFacility,
    BuildStarport,
    BuildSupplyDepot,
    BuildAcademyDisable,
    BuildArmoryDisable,
    BuildBunkerDisable,
    BuildComsatStationDisable,
    BuildFactoryDisable,
    BuildMissileTurretDisable,
    BuildNuclearSiloDisable,
    BuildScienceFacilityDisable,
    BuildStarportDisable,
    ProductBattlecruiser,
    ProductDropship,
    ProductFirebat,
    ProductGhost,
    ProductGoliath,
    ProductMarine,
    ProductMedic,
    ProductNuclear,
    ProductSciencevessle,
    ProductSCV,
    ProductSiegeTank,
    ProductValkyrie,
    ProductVulture,
    ProductWraith,
    ProductBattlecruiserDisable,
    ProductDropshipDisable,
    ProductFirebatDisable,
    ProductGhostDisable,
    ProductGoliathDisable,
    ProductMedicDisable,
    ProductSciencevessleDisable,
    ProductSiegeTankDisable,
    ProductValkyrieDisable,
    BuildHighStruct,
    BuildStruct,
    Clocking,
    DefensiveMatrix,
    EMP,
    Heal,
    Irradiate,
    Lockdown,
    Nuclear,
    OpticalFlare,
    Repair,
    Restoration,
    Scan,
    SiegeMod,
    SpiderMine,
    StimPack,
    TankMod,
    Unclocking,
    YamatoCanon,
    ClockingDisable,
    EMPDisable,
    IrradiateDisable,
    LockdownDisable,
    NuclearDisable,
    OpticalFlareDisable,
    RestorationDisable,
    SiegeModDisable,
    SpiderMineDisable,
    StimPackDisable,
    YamatoCanonDisable,
    UpgradeIrradiate,
    UpgradeScienceVessleEnergy,
    UpgradeAirAttack1,
    UpgradeAirAttack2,
    UpgradeAirAttack3,
    UpgradeAirDefence1,
    UpgradeAirDefence2,
    UpgradeAirDefence3,
    UpgradeBattlecruiserEnergy,
    UpgradeGhostClocking,
    UpgradeWraithClocking,
    UpgradeEMP,
    UpgradeGhostEnergy,
    UpgradeGoliathRange,
    UpgradeGroundBioAttack1,
    UpgradeGroundBioAttack2,
    UpgradeGroundBioAttack3,
    UpgradeGroundBioDefence1,
    UpgradeGroundBioDefence2,
    UpgradeGroundBioDefence3,
    UpgradeGroundMechAttack1,
    UpgradeGroundMechAttack2,
    UpgradeGroundMechAttack3,
    UpgradeGroundMechDefence1,
    UpgradeGroundMechDefence2,
    UpgradeGroundMechDefence3,
    UpgradeLockdown,
    UpgradeMarineRange,
    UpgradeMedicEnergy,
    UpgradeOpticalFlare,
    UpgradeRestoration,
    UpgradeStimPack,
    UpgradeSpiderMine,
    UpgradeSiegeMod,
    UpgradeVultureSpeed,
    UpgradeWraithEnergy,
    UpgradeYamatoCanon,
    UpgradeAirAttackDisable,
    UpgradeAirDefenceDisable,
    UpgradeGroundBioAttackDisable,
    UpgradeGroundBioDefenceDisable,
    UpgradeGroundMechAttackDisable,
    UpgradeGroundMechDefenceDisable,
};
public enum ButtonPageNum //
{
    None,
    Cancel,
    UnitsNormal,
    SCVNormal,
    MarineNormal,
    FirebatNormal,
    GhostNormal,
    MedicNormal,
    VultureNormal,
    SpiderMineNormal,
    SiegeTankNormal,
    GoliathNormal,
    WraithNormal,
    DropshipNormal,
    ScienceVesselNormal,
    BattlecruiserNormal,
    ValkyrieNormal,
    CommnadCenterNormal,
    ComSatStationNormal,
    NuclearSiloNormal,
    SupplyDepotNormal,
    RefineryNormal,
    BarracksNormal,
    EngineeringBayNormal,
    MissileTurretNormal,
    AcademyNormal,
    BunkerNormal,
    FactoryNormal,
    MachineShopNormal,
    StarportNormal,
    ControlTowerNormal,
    ScienceFacilityNormal,
    CovertOpsNormal,
    PhysicsLabNormal,
    ArmoryNormal,
    BuildStruct,
    BuildHighStruct
};
#endregion

public enum UnitName
{
    건설로봇,
    해병,
    화염방사병,
    유령,
    의무병,
    시체매,
    거미지뢰,
    공성전차,
    골리앗,
    망령,
    수송선,
    과학선,
    전투순양함,
    발키리,
};
public enum BuildingName
{
    사령부,
    통신위성중계소,
    핵격납고,
    보급고,
    정제소,
    병영,
    공학연구소,
    미사일포탑,
    사관학교,
    벙커,
    군수공장,
    기계실,
    우주공항,
    관제탑,
    과학시설,
    비밀작전실,
    물리연구실,
    무기고
};
public enum ClickMod
{
    Normal,
    Movement,
    SelectTarget,
    Building
}
[Flags]
public enum FlagsUnitState
{
    None = 0,
    Irradiate = 1 << 0,
    Unit = 1 << 1,
    Building = 1 << 2,
    Material = 1 << 3,
    Enemy = 1 << 4,
    Ground = 1 << 5,
}

[System.Serializable]
public class ButtonPage
{
    public ButtonStructNum[] buttonStructNum = new ButtonStructNum[9];
}