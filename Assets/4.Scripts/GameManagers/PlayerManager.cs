using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameObject Mineral;
    [SerializeField] private GameObject BespeneGas;
    [SerializeField] private GameObject Supply;

    TMP_Text textMineral;
    TMP_Text textBespeneGas;
    TMP_Text textSupply;

    private void Awake()
    {
        textMineral = Mineral.GetComponentInChildren<TMP_Text>();
        textBespeneGas = BespeneGas.GetComponentInChildren<TMP_Text>();
        textSupply = Supply.GetComponentInChildren<TMP_Text>();

        SetMineral();
        SetBespeneGas();
        SetSupply();
    }
    public void SetMineral()
    {
        textMineral.text = playerData.mineral.ToString();
    }
    public void SetBespeneGas()
    {
        textBespeneGas.text = playerData.bespeneGas.ToString();
    }
    public void SetSupply()
    {
        textSupply.text = playerData.nowSupply.ToString() + "/" + playerData.maxSupply.ToString();
    }
}
