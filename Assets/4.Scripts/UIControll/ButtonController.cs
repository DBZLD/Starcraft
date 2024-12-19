using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public ButtonManager[] m_buttonManager;
    [SerializeField] private ButtonStruct[] buttonStructList;
    [SerializeField] private ButtonPage[] buttonPageList;

    private UnitController m_UnitController;
    private BuildingController m_BuildingController;
    private ShowStateUI m_ShowStateUI;
    private PlayerManager m_PlayerManager;
    private void Awake()
    {
        m_UnitController = FindObjectOfType<UnitController>();
        m_BuildingController = FindObjectOfType<BuildingController>();
        m_ShowStateUI = FindObjectOfType<ShowStateUI>();
        m_PlayerManager = FindObjectOfType<PlayerManager>();

        for (int i = 0; i < 9 ; i++)
        { 
            m_buttonManager[i].buttonStruct = buttonStructList[0];
            int j = i;
            m_buttonManager[j].button.onClick.AddListener(() => SetOnButtonClick(m_buttonManager[j].buttonNum));

            EventTrigger eventTrigger = m_buttonManager[j].GetComponent<EventTrigger>();
            EventTrigger.Entry entry_Enter = new EventTrigger.Entry();
            EventTrigger.Entry entry_Exit = new EventTrigger.Entry();
            entry_Enter.eventID = EventTriggerType.PointerEnter;
            entry_Exit.eventID = EventTriggerType.PointerExit;

            entry_Enter.callback.AddListener((data) => { SetOnPointEnter(m_buttonManager[j].buttonNum); });
            entry_Exit.callback.AddListener((data) => { SetOnPointLeave(); });
            eventTrigger.triggers.Add(entry_Enter);
            eventTrigger.triggers.Add(entry_Exit);
        }
        StartCoroutine(SetButtonStruct());
    }
    private IEnumerator SetButtonStruct()
    {
        while (true)
        {
            if (m_UnitController.selectUnitList.Count == 1)
            {
                for(int i = 0; i < 9; i++)
                {
                    Debug.Log(IsCondition(buttonPageList[(int)m_UnitController.selectUnitList[0].nowButtonNumList].buttonStructNum[i]));
                    m_buttonManager[i].buttonStruct = buttonStructList[(int)IsCondition(buttonPageList[(int)m_UnitController.selectUnitList[0].nowButtonNumList].buttonStructNum[i])];
                }
            }
            else if (m_UnitController.selectUnitList.Count > 1)
            {
                for ( int i = 0; i < 9; i++)
                {
                    m_buttonManager[i].buttonStruct = buttonStructList[(int)buttonPageList[(int)ButtonPageNum.UnitsNormal].buttonStructNum[i]];
                }
            }
            else if (m_BuildingController.selectBuilding != null)
            {
                for (int i = 0; i < 9; i++)
                {
                    m_buttonManager[i].buttonStruct = buttonStructList[(int)IsCondition(buttonPageList[(int)m_BuildingController.selectBuilding.nowButtonNumList].buttonStructNum[i])];
                }
            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    m_buttonManager[i].buttonStruct = buttonStructList[(int)buttonPageList[(int)ButtonPageNum.None].buttonStructNum[i]];
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void SetOnButtonClick(int buttonNum)
    {
        Debug.Log(m_buttonManager[buttonNum].buttonStruct.buttonNum);
        if (m_UnitController.selectUnitList.Count == 1) { m_UnitController.ButtonFunction(m_buttonManager[buttonNum].buttonStruct); }
        else if(m_BuildingController.selectBuilding != null) { m_BuildingController.ButtonFunction(m_buttonManager[buttonNum].buttonStruct); }
    }
    public void SetOnPointEnter(int buttonNum)
    {
        m_ShowStateUI.OnButtonInfo(m_buttonManager[buttonNum].buttonStruct.buttonIfno);
    }
    public void SetOnPointLeave()
    {
        m_ShowStateUI.OffButtonInfo();
    }
    public ButtonStruct GetButtonStruct(ButtonPageNum buttonPage, int i)
    {
        return buttonStructList[(int)buttonPageList[(int)buttonPage].buttonStructNum[i]];
    }
    public ButtonStruct GetButtonStruct(int buttonPageNum, int i) // button struct구하기
    {
        return buttonStructList[(int)buttonPageList[buttonPageNum].buttonStructNum[i]];
    }

    public void SetButtonImage(ButtonPageNum buttonPage)  // 버튼 이미지 설정
    {
        SpriteState myState;
        for(int i = 0; i < 9; i++)
        {
            if(buttonStructList[(int)buttonPageList[(int)buttonPage].buttonStructNum[i]] == buttonStructList[(int)ButtonStructNum.None]) 
            { 
                m_buttonManager[i].button.interactable = false; 
            }
            else { m_buttonManager[i].button.interactable = true; }
            m_buttonManager[i].image.sprite = buttonStructList[(int)IsCondition(buttonPageList[(int)buttonPage].buttonStructNum[i])].buttonNormalImage;

            myState.pressedSprite = buttonStructList[(int)IsCondition(buttonPageList[(int)buttonPage].buttonStructNum[i])].buttonPressedImage;
            m_buttonManager[i].button.spriteState = myState;
        }
    }
    public ButtonStructNum IsCondition(ButtonStructNum buttonStruct)
    {
        switch (buttonStruct)
        {
            case ButtonStructNum.BuildAcademy :
                if(m_PlayerManager.isBarracks == false) { return ButtonStructNum.BuildAcademyDisable; }
                break;
            case ButtonStructNum.BuildArmory:
                if(m_PlayerManager.isFactory == false) { return ButtonStructNum.BuildArmoryDisable; }
                break;
            case ButtonStructNum.BuildBunker:
                if(m_PlayerManager.isBarracks == false) { return ButtonStructNum.BuildBunkerDisable; }
                break;
            case ButtonStructNum.BuildComsatStation:
                if (m_PlayerManager.isAcademy == false) { return ButtonStructNum.BuildComsatStationDisable; }
                break;
            case ButtonStructNum.BuildFactory:
                if (m_PlayerManager.isBarracks == false) { return ButtonStructNum.BuildFactoryDisable; }
                break;
            case ButtonStructNum.BuildMissileTurret:
                if (m_PlayerManager.isEngineeringBay == false) { return ButtonStructNum.BuildMissileTurretDisable; }
                break;
            case ButtonStructNum.BuildNuclearSilo:
                if (m_PlayerManager.isScienceFacility == false || m_PlayerManager.isCovertOps == false) { return ButtonStructNum.BuildNuclearSiloDisable; }
                break;
            case ButtonStructNum.BuildScienceFacility:
                if (m_PlayerManager.isStarport == false) { return ButtonStructNum.BuildScienceFacilityDisable; }
                break;
            case ButtonStructNum.BuildStarport:
                if (m_PlayerManager.isFactory == false) { return ButtonStructNum.BuildStarportDisable; }
                break;
            case ButtonStructNum.ProductBattlecruiser:
                if (m_PlayerManager.isControlTower == false || m_PlayerManager.isScienceFacility == false || m_PlayerManager.isPhysicLab == false) { return ButtonStructNum.ProductBattlecruiserDisable; }
                break;
            case ButtonStructNum.ProductDropship:
                if (m_PlayerManager.isControlTower == false) { return ButtonStructNum.ProductDropshipDisable; }
                break;
            case ButtonStructNum.ProductSciencevessle:
                if (m_PlayerManager.isControlTower == false || m_PlayerManager.isScienceFacility == false) { return ButtonStructNum.ProductSciencevessleDisable; }
                break;
            case ButtonStructNum.ProductValkyrie:
                if (m_PlayerManager.isControlTower == false || m_PlayerManager.isArmory == false) { return ButtonStructNum.ProductValkyrieDisable; }
                break;
            case ButtonStructNum.ProductFirebat:
                if (m_PlayerManager.isAcademy == false) { return ButtonStructNum.ProductFirebatDisable; }
                break;
            case ButtonStructNum.ProductMedic:
                if (m_PlayerManager.isAcademy == false) { return ButtonStructNum.ProductMedicDisable; }
                break;
            case ButtonStructNum.ProductGhost:
                if (m_PlayerManager.isAcademy == false || m_PlayerManager.isScienceFacility == false || m_PlayerManager.isCovertOps == false) { return ButtonStructNum.ProductGhostDisable; }
                break;
            case ButtonStructNum.ProductSiegeTank:
                if (m_PlayerManager.isMechineShop == false) { return ButtonStructNum.ProductSiegeTankDisable; }
                break;
            case ButtonStructNum.ProductGoliath:
                if (m_PlayerManager.isArmory == false) { return ButtonStructNum.ProductGoliath; }
                break;
            case ButtonStructNum.ProductNuclear:
                if (m_PlayerManager.isNuclear == true) { return ButtonStructNum.None; }
                break;
            case ButtonStructNum.Clocking:
                if (m_PlayerManager.isClockingUpgrade == false) { return ButtonStructNum.ClockingDisable; }
                break;
            case ButtonStructNum.EMP:
                if (m_PlayerManager.isEMPUpgrade == false) { return ButtonStructNum.EMPDisable; }
                break;
            case ButtonStructNum.Irradiate:
                if (m_PlayerManager.isIrradiateUpgrade == false) { return ButtonStructNum.IrradiateDisable; }
                break;
            case ButtonStructNum.Lockdown:
                if (m_PlayerManager.isLockdownUpgrade == false) { return ButtonStructNum.LockdownDisable; }
                break;
            case ButtonStructNum.Nuclear:
                if (m_PlayerManager.isNuclear == false) { return ButtonStructNum.NuclearDisable; }
                break;
            case ButtonStructNum.OpticalFlare:
                if (m_PlayerManager.isOpticalFlareUpgrade == false) { return ButtonStructNum.OpticalFlareDisable; }
                break;
            case ButtonStructNum.Restoration:
                if (m_PlayerManager.isRestorationUpgrade == false) { return ButtonStructNum.RestorationDisable; }
                break;
            case ButtonStructNum.SpiderMine:
                if (m_PlayerManager.isSpiderMineUpgrade == false) { return ButtonStructNum.SpiderMineDisable; }
                break;
            case ButtonStructNum.StimPack:
                if (m_PlayerManager.isStimpackUpgrade == false) { return ButtonStructNum.StimPackDisable; }
                break;
            case ButtonStructNum.UpgradeAirAttack1:
                if (m_PlayerManager.isScienceFacility == false)
                {
                    if (m_PlayerManager.AirDamageUpgrade == 0) { return ButtonStructNum.UpgradeAirAttack1; }
                    else if (m_PlayerManager.AirDamageUpgrade >= 1) { return ButtonStructNum.UpgradeAirAttackDisable; }
                }
                else
                {
                    if (m_PlayerManager.AirDamageUpgrade == 0) { return ButtonStructNum.UpgradeAirAttack1; }
                    else if (m_PlayerManager.AirDamageUpgrade == 1) { return ButtonStructNum.UpgradeAirAttack2; }
                    else if (m_PlayerManager.AirDamageUpgrade == 2) { return ButtonStructNum.UpgradeAirAttack3; }
                    else if (m_PlayerManager.AirDamageUpgrade == 3) { return ButtonStructNum.None; }
                  
                } break;
            case ButtonStructNum.UpgradeAirDefence1:
                if (m_PlayerManager.isScienceFacility == false)
                {
                    if (m_PlayerManager.AirDefenceUpgrade == 0) { return ButtonStructNum.UpgradeAirDefence1; }
                    else if (m_PlayerManager.AirDefenceUpgrade >= 1) { return ButtonStructNum.UpgradeAirDefenceDisable; }
                }
                else
                {
                    if (m_PlayerManager.AirDefenceUpgrade == 0) { return ButtonStructNum.UpgradeAirDefence1; }
                    else if (m_PlayerManager.AirDefenceUpgrade == 1) { return ButtonStructNum.UpgradeAirDefence2; }
                    else if (m_PlayerManager.AirDefenceUpgrade == 2) { return ButtonStructNum.UpgradeAirDefence3; }
                    else if (m_PlayerManager.AirDefenceUpgrade == 3) { return ButtonStructNum.None; }

                }
                break;
            case ButtonStructNum.UpgradeGroundBioAttack1:
                if (m_PlayerManager.isScienceFacility == false)
                {
                    if (m_PlayerManager.groundBioDamageUpgrade == 0) { return ButtonStructNum.UpgradeGroundBioAttack1; }
                    else if (m_PlayerManager.groundBioDamageUpgrade >= 1) { return ButtonStructNum.UpgradeGroundBioAttackDisable; }
                }
                else
                {
                    if (m_PlayerManager.groundBioDamageUpgrade == 0) { return ButtonStructNum.UpgradeGroundBioAttack1; }
                    else if (m_PlayerManager.groundBioDamageUpgrade == 1) { return ButtonStructNum.UpgradeGroundBioAttack2; }
                    else if (m_PlayerManager.groundBioDamageUpgrade == 2) { return ButtonStructNum.UpgradeGroundBioAttack3; }
                    else if (m_PlayerManager.groundBioDamageUpgrade == 3) { return ButtonStructNum.None; }

                }
                break;
            case ButtonStructNum.UpgradeGroundBioDefence1:
                if (m_PlayerManager.isScienceFacility == false)
                {
                    if (m_PlayerManager.groundBioDefenceUpgrade == 0) { return ButtonStructNum.UpgradeGroundBioDefence1; }
                    else if (m_PlayerManager.groundBioDefenceUpgrade >= 1) { return ButtonStructNum.UpgradeGroundBioDefenceDisable; }
                }
                else
                {
                    if (m_PlayerManager.groundBioDefenceUpgrade == 0) { return ButtonStructNum.UpgradeGroundBioDefence1; }
                    else if (m_PlayerManager.groundBioDefenceUpgrade == 1) { return ButtonStructNum.UpgradeGroundBioDefence2; }
                    else if (m_PlayerManager.groundBioDefenceUpgrade == 2) { return ButtonStructNum.UpgradeGroundBioDefence3; }
                    else if (m_PlayerManager.groundBioDefenceUpgrade == 3) { return ButtonStructNum.None; }

                }
                break;
            case ButtonStructNum.UpgradeGroundMechAttack1:
                if (m_PlayerManager.isScienceFacility == false)
                {
                    if (m_PlayerManager.groundMechDamageUpgrade == 0) { return ButtonStructNum.UpgradeGroundMechAttack1; }
                    else if (m_PlayerManager.groundMechDamageUpgrade >= 1) { return ButtonStructNum.UpgradeGroundMechAttackDisable; }
                }
                else
                {
                    if (m_PlayerManager.groundMechDamageUpgrade == 0) { return ButtonStructNum.UpgradeGroundMechAttack1; }
                    else if (m_PlayerManager.groundMechDamageUpgrade == 1) { return ButtonStructNum.UpgradeGroundMechAttack2; }
                    else if (m_PlayerManager.groundMechDamageUpgrade == 2) { return ButtonStructNum.UpgradeGroundMechAttack3; }
                    else if (m_PlayerManager.groundMechDamageUpgrade == 3) { return ButtonStructNum.None; }

                }
                break;
            case ButtonStructNum.UpgradeGroundMechDefence1:
                if (m_PlayerManager.isScienceFacility == false)
                {
                    if (m_PlayerManager.groundMechDefenceUpgrade == 0) { return ButtonStructNum.UpgradeGroundMechDefence1; }
                    else if (m_PlayerManager.groundMechDefenceUpgrade >= 1) { return ButtonStructNum.UpgradeGroundMechDefenceDisable; }
                }
                else
                {
                    if (m_PlayerManager.groundMechDefenceUpgrade == 0) { return ButtonStructNum.UpgradeGroundMechDefence1; }
                    else if (m_PlayerManager.groundMechDefenceUpgrade == 1) { return ButtonStructNum.UpgradeGroundMechDefence2; }
                    else if (m_PlayerManager.groundMechDefenceUpgrade == 2) { return ButtonStructNum.UpgradeGroundMechDefence3; }
                    else if (m_PlayerManager.groundMechDefenceUpgrade == 3) { return ButtonStructNum.None; }

                }
                break;
        }
        return buttonStruct;
    }
}
