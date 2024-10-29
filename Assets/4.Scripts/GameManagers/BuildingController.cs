using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField]
    public BuildingManager selectBuilding;
    [SerializeField]
    public List<BuildingManager> allBuildingList;

    public void ClickSelectBuilding(BuildingManager NewBuilding)
    {
        UnselectBuilding();

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

    public void AddUnitList(BuildingManager NewBuilding)
    {
        allBuildingList.Add(NewBuilding);
    }
}
