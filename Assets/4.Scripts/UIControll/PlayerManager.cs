using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private TMP_Text Mineral;
    [SerializeField] private TMP_Text BespeneGas;
    [SerializeField] private TMP_Text Supply;

    private void Awake()
    {
        SetMineral();
        SetBespeneGas();
        SetSupply();
    }
    public void SetMineral()
    {
        Mineral.text = playerData.mineral.ToString();
    }
    public void SetBespeneGas()
    {
        BespeneGas.text = playerData.bespeneGas.ToString();
    }
    public void SetSupply()
    {
        Supply.text = playerData.nowSupply.ToString() + "/" + playerData.maxSupply.ToString();
    }
}
