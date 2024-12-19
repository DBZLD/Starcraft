using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField]
    public BuildingManager selectBuilding;
    [SerializeField]
    public List<BuildingManager> allBuildingList;

    private PlayerManager m_PlayerManager;
    private void Awake()
    {
        m_PlayerManager = GetComponent<PlayerManager>();
    }

    public void ClickSelectBuilding(BuildingManager NewBuilding)
    {
        if (selectBuilding != null) { UnselectBuilding(); }

        SelectBuilding(NewBuilding);
    }
    private void SelectBuilding(BuildingManager NewBuilding)
    {
        NewBuilding.MarkedBuilding();

        selectBuilding = NewBuilding;
    }
    public void UnselectBuilding()
    {
        selectBuilding.UnMarkedBuilding();

        selectBuilding = null;
    }
    public void ProductUnit(GameObject prefab, int nTime, int nSupply, int nMineral, int nVespene)
    {
        selectBuilding.productList = StartCoroutine(selectBuilding.ProductionUnit(prefab, nTime, nSupply, nMineral, nVespene));
    }
    public void AddUnitList(BuildingManager NewBuilding)
    {
        allBuildingList.Add(NewBuilding);
    }
    public void ButtonFunction(ButtonStruct buttonStruct)
    {
        switch (buttonStruct.buttonNum)
        {
            case ButtonStructNum.Cancle:
                selectBuilding.nowButtonNumList = selectBuilding.GetData().mainButtonPage;
                if (selectBuilding.productList != null) { StopCoroutine(selectBuilding.productList); }
                break;
            case ButtonStructNum.ProductSCV:
            case ButtonStructNum.ProductDropship:
            case ButtonStructNum.ProductFirebat:
            case ButtonStructNum.ProductGhost:
            case ButtonStructNum.ProductGoliath:
            case ButtonStructNum.ProductMarine:
            case ButtonStructNum.ProductMedic:
            case ButtonStructNum.ProductNuclear:
            case ButtonStructNum.ProductSciencevessle:
            case ButtonStructNum.ProductSiegeTank:
            case ButtonStructNum.ProductValkyrie:
            case ButtonStructNum.ProductVulture:
            case ButtonStructNum.ProductWraith:
            case ButtonStructNum.ProductBattlecruiser:
                if (selectBuilding.productList != null || buttonStruct.needMineral > m_PlayerManager.GetMineral() || buttonStruct.needVespeneGas > m_PlayerManager.GetVespeneGas() || buttonStruct.needSupply > m_PlayerManager.GetMaxSupply() - m_PlayerManager.GetNowSupply())
                { Debug.Log("not enought material"); return; }
                ProductUnit(buttonStruct.objectPrefab, buttonStruct.needTime, buttonStruct.needSupply, buttonStruct.needMineral, buttonStruct.needVespeneGas);
                break;
            default:
                break;
        }
    }
}
