using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HoldingMaterialManager : MonoBehaviour
{
    [SerializeField] private GameObject MineralChunk;
    [SerializeField] private GameObject VespeneGasTank;

    private GameObject m_GameManager;
    public MaterialType holdingMaterialType;
    public int holdingMaterialValue;

    private void Awake()
    {
        MineralChunk.SetActive(false);
        VespeneGasTank.SetActive(false);
        m_GameManager = GameObject.Find("GameManager");
    }
    public void GatherMaterial(MaterialType materialType, int materialValue) // 자원 채취 시 호출
    {
        holdingMaterialType = materialType;
        holdingMaterialValue = materialValue;
        if(materialType == MaterialType.Mineral) { MineralChunk.SetActive(true); }
        if(materialType == MaterialType.VespeneGas) { VespeneGasTank.SetActive(true); }
    }
    public void PutMaterial() // 자원 보관 시 호출
    {
        m_GameManager.GetComponent<PlayerManager>().IncreaseMaterial(holdingMaterialType, holdingMaterialValue);
        holdingMaterialType = MaterialType.None;
        holdingMaterialValue = 0;
        MineralChunk.SetActive(false);
        VespeneGasTank.SetActive(false);
    }
}
