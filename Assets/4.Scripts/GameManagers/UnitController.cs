using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class UnitController : MonoBehaviour
{
    [SerializeField]
    public List<UnitManager> selectUnitList;
    [SerializeField]
    public List<UnitManager> allUnitList;

    private NavMeshManager m_NavMeshManager;
    private void Awake()
    {
        m_NavMeshManager = GetComponent<NavMeshManager>();
    }

    public void ClickSelectUnit(UnitManager NewUnit)
    {
        UnselectAll();

        SelectUnit(NewUnit);
    }
    public void ShiftClickSelectUnit(UnitManager NewUnit)
    {
        if (selectUnitList.Contains(NewUnit))
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
        if (!selectUnitList.Contains(NewUnit))
        {
            SelectUnit(NewUnit);
        }
    }
    private void SelectUnit(UnitManager NewUnit)
    {
        NewUnit.MarkedUnit();

        selectUnitList.Add(NewUnit);
    }
    private void UnselectUnit(UnitManager NewUnit)
    {
        NewUnit.UnMarkedUnit();

        selectUnitList.Remove(NewUnit);
    }
    public void UnselectAll()
    {
        for (int i = 0; i < selectUnitList.Count; i++)
        {
            selectUnitList[i].UnMarkedUnit();
        }

        selectUnitList.Clear();
    }

    public void AddUnitList(UnitManager NewUnit)
    {
        allUnitList.Add(NewUnit);
    }
    public int UIPriority()
    {
        List<int> UI = new List<int>();

        for(int i = 0; i < selectUnitList.Count; i++)
        {
            UI.Add(selectUnitList[i].uiPriority);
        }
        int MaxValue = UI.Max();
        int nReturn = UI.IndexOf(MaxValue);

        return nReturn;
    }
    public void MoveSelectedUnit(Vector3 end)
    {
        m_NavMeshManager.NavMeshBake();
        for (int i = 0; i < selectUnitList.Count; i++)
        {
            if (selectUnitList[i].coroutineList != null) { StopCoroutine(selectUnitList[i].coroutineList); }
            selectUnitList[i].coroutineList = StartCoroutine(selectUnitList[i].MoveCoroutine(end));
        }
    }
    public void MoveSelectedUnit(GameObject target)
    {
        m_NavMeshManager.NavMeshBake();
        for (int i = 0; i < selectUnitList.Count; i++)
        {
            if (selectUnitList[i].coroutineList != null) { StopCoroutine(selectUnitList[i].coroutineList); }
            selectUnitList[i].coroutineList = StartCoroutine(selectUnitList[i].MoveCoroutine(target));
        }
    }
    public void StopSelectedUnit()
    {
        m_NavMeshManager.NavMeshBake();
        for (int i = 0; i < selectUnitList.Count; i++)
        {
            if (selectUnitList[i].coroutineList != null) { StopCoroutine(selectUnitList[i].coroutineList); }
            selectUnitList[i].StopMove();
        }
    }
    public void HoldSelectedUnit()
    {
        m_NavMeshManager.NavMeshBake();
        for (int i = 0; i < selectUnitList.Count; i++)
        {
            if (selectUnitList[i].coroutineList != null) { StopCoroutine(selectUnitList[i].coroutineList); }
            selectUnitList[i].coroutineList = StartCoroutine(selectUnitList[i].HoldCoroutine());
        }
    }
    public void AttackSelectedUnit(Vector3 end)
    {
        m_NavMeshManager.NavMeshBake();
        for (int i = 0; i < selectUnitList.Count; i++)
        {
            if (selectUnitList[i].coroutineList != null) { StopCoroutine(selectUnitList[i].coroutineList); }
            selectUnitList[i].coroutineList = StartCoroutine(selectUnitList[i].AttackCoroutine(end));
        }
    }
    public void AttackSelectedUnit(GameObject target)
    {
        m_NavMeshManager.NavMeshBake();
        for (int i = 0; i < selectUnitList.Count; i++)
        {
            if (selectUnitList[i].coroutineList != null) { StopCoroutine(selectUnitList[i].coroutineList); }
            selectUnitList[i].coroutineList = StartCoroutine(selectUnitList[i].AttackCoroutine(target));
        }
    } 
    public void GatheringSelectedUnit(GameObject target)
    {
        m_NavMeshManager.NavMeshBake();
        for (int i = 0; i < selectUnitList.Count; i++)
        {
            if (selectUnitList[i].coroutineList != null) { StopCoroutine(selectUnitList[i].coroutineList); }
            selectUnitList[i].coroutineList = StartCoroutine(selectUnitList[i].GatheringCoroutine(target));
        }
    }
    public int CountSelectedUnit()
    {
        return selectUnitList.Count;
    }
}
