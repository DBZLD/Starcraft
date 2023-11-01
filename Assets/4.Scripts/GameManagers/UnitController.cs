using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitController : MonoBehaviour
{
    [SerializeField]
    public List<UnitManager> SelectUnitList;
    [SerializeField]
    public List<UnitManager> AllUnitList;

    public void ClickSelectUnit(UnitManager NewUnit)
    {
        UnselectAll();

        SelectUnit(NewUnit);
    }
    public void ShiftClickSelectUnit(UnitManager NewUnit)
    {
        if (SelectUnitList.Contains(NewUnit))
        {
            UnselectUnit(NewUnit);
        }
        else
        {
            SelectUnit(NewUnit);
        }
    }
    public void DragSelectUnit(UnitManager NewUnit)
    {
        if (!SelectUnitList.Contains(NewUnit))
        {
            SelectUnit(NewUnit);
        }
    }

    private void SelectUnit(UnitManager NewUnit)
    {
        NewUnit.MarkedUnit();

        SelectUnitList.Add(NewUnit);
    }
    private void UnselectUnit(UnitManager NewUnit)
    {
        NewUnit.UnMarkedUnit();

        SelectUnitList.Remove(NewUnit);
    }
    public void UnselectAll()
    {
        for (int i = 0; i < SelectUnitList.Count; i++)
        {
            SelectUnitList[i].UnMarkedUnit();
        }

        SelectUnitList.Clear();
    }

    public void AddUnitList(UnitManager NewUnit)
    {
        AllUnitList.Add(NewUnit);
    }
    public int UIPriority()
    {
        List<int> UI = new List<int>();

        for(int i = 0; i < SelectUnitList.Count; i++)
        {
            UI.Add(SelectUnitList[i].uiPriority);
        }
        int MaxValue = UI.Max();
        int nReturn = UI.IndexOf(MaxValue);

        return nReturn;
    }
    public void MoveSelectedUnit(Vector3 end)
    {
        for (int i = 0; i < SelectUnitList.Count; i++)
        {
            SelectUnitList[i].Moveto(end);
        }
    }
    public void StopSelectedUnit()
    {
        for (int i = 0; i < SelectUnitList.Count; i++)
        {
            SelectUnitList[i].StopMove();
        }
    }
    public void AttackSelectedUnit()
    {

    }

    public bool IsSelectedUnit()
    {
        if (SelectUnitList.Count <= 0) { return false; }
        return true;
    }
}
