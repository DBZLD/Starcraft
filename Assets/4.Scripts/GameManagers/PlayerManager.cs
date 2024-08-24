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

    private int nowMineral;
    private int nowBespeneGas;
    private int nowSupply;
    private int maxSupply;

    public void VariationMineral(int Mineral, bool addition)
    {
        if(addition == true)
        {
            nowMineral += Mineral;
        }
        else if(addition == false)
        {
            nowMineral -= Mineral;
        }
    }
    public void VariationBespeneGas(int BespeneGas, bool addition)
    {
        if (addition == true)
        {
            nowBespeneGas += BespeneGas;
        }
        else if (addition == false)
        {
            nowBespeneGas -= BespeneGas;
        }
    }
    public void VariationNowSupply(int NowSupply, bool addition)
    {
        if (addition == true)
        {
            nowSupply += NowSupply;
        }
        else if (addition == false)
        {
            nowSupply -= NowSupply;
        }
    }
    public void VariationMaxSupply(int MaxSupply, bool addition)
    {
        if (addition == true)
        {
            nowSupply += MaxSupply;
        }
        else if (addition == false)
        {
            nowSupply -= MaxSupply;
        }
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
