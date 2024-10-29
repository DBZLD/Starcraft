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

    public void IncreaseMaterial(MaterialType materialType, int value)
    {
        if(materialType == MaterialType.Mineral)
        {
            nowMineral += value;
        }
        if (materialType == MaterialType.BespeneGas)
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
        if (materialType == MaterialType.BespeneGas)
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

    public int GetMineral()
    {
        return nowMineral;
    }
    public int GetBespeneGas()
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
