using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;

public class UnitController : MonoBehaviour
{
    [SerializeField]
    public List<UnitManager> selectUnitList;
    [SerializeField]
    public List<UnitManager> allUnitList;

    private NavMeshManager m_NavMeshManager;
    private ClickManager m_ClickManager;
    private PlayerManager m_PlayerManager;
    private void Awake()
    {
        m_NavMeshManager = GetComponent<NavMeshManager>();
        m_ClickManager = GetComponent<ClickManager>();
        m_PlayerManager = GetComponent<PlayerManager>();
    }

    public void UpgradeUnit(UpgradeType upgradeType, bool isAttack)
    {
        for(int i = 0; i < allUnitList.Count; i++)
        {
            if(allUnitList[i].GetData().upgradeType == upgradeType)
            {
                if(isAttack == true)
                {
                    allUnitList[i].damageUpgradeCount++;
                }
                else
                {
                    allUnitList[i].defenceUpgradeCount++;
                }
            }
        }
        m_PlayerManager.UpgradeComplete(upgradeType, isAttack);
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

    public void MoveSelectedUnit(Vector3 end)
    {
        m_NavMeshManager.NavMeshBake();
        for (int i = 0; i < selectUnitList.Count; i++)
        {
            if (selectUnitList[i].coroutineList != null) { StopCoroutine(selectUnitList[i].coroutineList); }
            if (selectUnitList[i].GetData().isMove == true)
            {
                selectUnitList[i].coroutineList = StartCoroutine(selectUnitList[i].MoveCoroutine(end));
            }
        }
    }
    public void MoveSelectedUnit(GameObject target)
    {
        m_NavMeshManager.NavMeshBake();
        for (int i = 0; i < selectUnitList.Count; i++)
        {
            if (selectUnitList[i].coroutineList != null) { StopCoroutine(selectUnitList[i].coroutineList); }
            if (selectUnitList[i].GetData().isMove == true)
            {
                selectUnitList[i].coroutineList = StartCoroutine(selectUnitList[i].MoveCoroutine(target));
            }
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
            if (selectUnitList[i].GetData().isAttack == true)
            {
                selectUnitList[i].coroutineList = StartCoroutine(selectUnitList[i].HoldCoroutine());
            }
        }
    }
    public void PatrolSelectedUnit(Vector3 end)
    {
        m_NavMeshManager.NavMeshBake();
        for (int i = 0; i < selectUnitList.Count; i++)
        {
            if (selectUnitList[i].coroutineList != null) { StopCoroutine(selectUnitList[i].coroutineList); }
            selectUnitList[i].coroutineList = StartCoroutine(selectUnitList[i].PatrolCoroutine(end));
        }
    }
    public void AttackSelectedUnit(Vector3 end)
    {
        m_NavMeshManager.NavMeshBake();
        for (int i = 0; i < selectUnitList.Count; i++)
        {
            if (selectUnitList[i].coroutineList != null) { StopCoroutine(selectUnitList[i].coroutineList); }
            if (selectUnitList[i].GetData().isAttack == true)
            {
                selectUnitList[i].coroutineList = StartCoroutine(selectUnitList[i].AttackCoroutine(end));
            }
        }
    }
    public void AttackSelectedUnit(GameObject target)
    {
        m_NavMeshManager.NavMeshBake();
        for (int i = 0; i < selectUnitList.Count; i++)
        {
            if (selectUnitList[i].coroutineList != null) { StopCoroutine(selectUnitList[i].coroutineList); }
            if (selectUnitList[i].GetData().isAttack == true)
            {
                selectUnitList[i].coroutineList = StartCoroutine(selectUnitList[i].AttackCoroutine(target));
            }
        }
    } 
    public void GatheringSelectedUnit(GameObject target)
    {
        m_NavMeshManager.NavMeshBake();
        for (int i = 0; i < selectUnitList.Count; i++)
        {
            if (selectUnitList[i].coroutineList != null) { StopCoroutine(selectUnitList[i].coroutineList); }
            if (selectUnitList[i].GetData().isGathering == true)
            {
                selectUnitList[i].coroutineList = StartCoroutine(selectUnitList[i].GatheringCoroutine(target));
            }
        }
    }
    public void ButtonFunction(ButtonStruct buttonStruct)
    {
        switch (buttonStruct.buttonNum)
        {
            case ButtonStructNum.Cancle:
                selectUnitList[0].nowButtonNumList = selectUnitList[0].GetData().mainButtonPage;
                break;
            case ButtonStructNum.Move:
                m_ClickManager.keyInput = 1;
                break;
            case ButtonStructNum.Attack:
                m_ClickManager.keyInput = 2;
                break;
            case ButtonStructNum.Patrol:
                m_ClickManager.keyInput = 3;
                break;
            case ButtonStructNum.Gathering:
                m_ClickManager.keyInput = 4;
                break;
            case ButtonStructNum.Stop:
                StopSelectedUnit();
                break;
            case ButtonStructNum.Hold:
                HoldSelectedUnit();
                break;
            case ButtonStructNum.BuildStruct:
                selectUnitList[0].nowButtonNumList = ButtonPageNum.BuildStruct;
                break;
            case ButtonStructNum.BuildHighStruct:
                selectUnitList[0].nowButtonNumList = ButtonPageNum.BuildHighStruct;
                break;
            default:
                break;

        }
    }
    
    public int CountSelectedUnit()
    {
        return selectUnitList.Count;
    }
}
