using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;

public class PlayerManager : MonoBehaviour
{
    [Header ("PlayerData")]
    [SerializeField] private int nowMineral;
    [SerializeField] private int nowBespeneGas;
    [SerializeField] private int nowSupply;
    [SerializeField] private int maxSupply;

    [Header ("Upgrade")]
    public int groundBioDamageUpgrade;
    public int groundMechDamageUpgrade;
    public int AirDamageUpgrade;
    public int groundBioDefenceUpgrade;
    public int groundMechDefenceUpgrade;
    public int AirDefenceUpgrade;
    public bool isClockingUpgrade;
    public bool isEMPUpgrade;
    public bool isIrradiateUpgrade;
    public bool isLockdownUpgrade;
    public bool isOpticalFlareUpgrade;
    public bool isRestorationUpgrade;
    public bool isSiegeModUpgrade;
    public bool isSpiderMineUpgrade;
    public bool isStimpackUpgrade;

    [Header ("Condition")]
    public bool isNuclear;
    public bool isEngineeringBay;
    public bool isBarracks;
    public bool isAcademy;
    public bool isFactory;
    public bool isArmory;
    public bool isStarport;
    public bool isScienceFacility;
    public bool isPhysicLab;
    public bool isCovertOps;
    public bool isMechineShop;
    public bool isControlTower;

    private UnitController m_unitController;

    private void Start()
    {
        m_unitController = GetComponent<UnitController>();
        ResetPlayerData();
    }
    public void UpgradeComplete(UpgradeType upgradeType, bool isDamage)
    {
        if(upgradeType == UpgradeType.GroundBio)
        {
            if(isDamage == true)
            {
                groundBioDamageUpgrade++;
            }
            else
            {
                groundBioDefenceUpgrade++;
            }
        }
        else if (upgradeType == UpgradeType.GroundMech)
        {
            if (isDamage == true)
            {
                groundMechDamageUpgrade++;
            }
            else
            {
                groundMechDefenceUpgrade++;
            }
        }
        else if (upgradeType == UpgradeType.Air)
        {
            if (isDamage == true)
            {
                AirDamageUpgrade++;
            }
            else
            {
                AirDefenceUpgrade++;
            }
        }
    }

    public void IncreaseMaterial(MaterialType materialType, int value)
    {
        if(materialType == MaterialType.Mineral)
        {
            nowMineral += value;
        }
        if (materialType == MaterialType.VespeneGas)
        {
            nowBespeneGas += value;
        }
    }
    public void DecreaseMaterial(MaterialType materialType, int value)
    {
        if (materialType == MaterialType.Mineral)
        {
            nowMineral -= value;
        }
        if (materialType == MaterialType.VespeneGas)
        {
            nowBespeneGas -= value;
        }
    }
    public void IncreaseNowSupply(int value)
    {
        nowSupply += value;
    }
    public void DecreaseNowSupply(int value)
    {
        nowSupply -= value;
    }
    public void IncreaseMaxSupply(int value)
    {
        maxSupply += value;
    }
    public void DecreaseMaxSupply(int value)
    {
        maxSupply -= value;
    }
    public void ResetPlayerData()
    {
        nowMineral = 0;
        nowBespeneGas = 0;
        nowSupply = 0;
        maxSupply = 200;

        AirDamageUpgrade = 0;
        groundBioDamageUpgrade = 0;
        groundMechDamageUpgrade = 0;
        AirDefenceUpgrade = 0;
        groundBioDefenceUpgrade = 0;
        groundMechDefenceUpgrade = 0;

        isClockingUpgrade = false;
        isEMPUpgrade = false;
        isIrradiateUpgrade = false;
        isLockdownUpgrade = false;  
        isOpticalFlareUpgrade = false;
        isRestorationUpgrade = false;
        isSiegeModUpgrade = false;
        isSpiderMineUpgrade = false;
        isStimpackUpgrade = false;
        isNuclear = false;
        isEngineeringBay = false;
        isBarracks = false;
        isAcademy = false;
        isFactory = false;
        isArmory = false;
        isStarport = false;
        isScienceFacility = false;
        isPhysicLab = false;
        isCovertOps = false;
        isMechineShop = false;
        isControlTower = false;
}

    public int GetMineral()
    {
        return nowMineral;
    }
    public int GetVespeneGas()
    {
        return nowBespeneGas;
    }
    public int GetNowSupply()
    {
        return nowSupply;
    }
    public int GetMaxSupply()
    {
        return maxSupply;
    }
}
