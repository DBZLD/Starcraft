using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField]
    public BuildingManager SelectingBuilding;
    [SerializeField]
    public List<BuildingManager> AllBuildingList;

    public void ClickSelectBuilding(BuildingManager NewBuilding)
    {
        SelectBuilding(NewBuilding);
    }
    private void SelectBuilding(BuildingManager NewBuilding)
    {
        NewBuilding.MarkedBuilding();

        SelectingBuilding = NewBuilding;
    }
    public void UnselectBuilding()
    {
        SelectingBuilding.UnMarkedBuilding();

        SelectingBuilding = null;
    }

    public void AddUnitList(BuildingManager NewBuilding)
    {
        AllBuildingList.Add(NewBuilding);
    }
}
