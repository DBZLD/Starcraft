using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour
{
    [SerializeField] private LayerMask layerEnemy;
    [SerializeField] private LayerMask layerAlly;
    [SerializeField] private LayerMask layerThird;

    private Camera MainCamera;
    private UnitController m_UnitController;
    private BuildingController m_BuildingCountroller;
    private MaterialController m_MaterialController;

    public int keyInput = 0;

    private void Awake()
    {
        MainCamera = Camera.main;

        m_UnitController = GetComponent<UnitController>();
        m_BuildingCountroller = GetComponent<BuildingController>();
        m_MaterialController = GetComponent<MaterialController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            m_UnitController.StopSelectedUnit();
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            m_UnitController.HoldSelectedUnit();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            keyInput = 1;
        }
        else if (Input.GetKey(KeyCode.G))
        {
            keyInput = 2;
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            m_UnitController.SelectUnitList[0].nowHp += 10;
        }
        else
        {
            keyInput = 0;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit hit;

                Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerAlly))
                {
                    if (hit.collider.CompareTag("Unit"))
                    {
                        if (hit.transform.GetComponent<UnitManager>() == null) { return; }
                        if (m_BuildingCountroller.SelectingBuilding != null) { m_BuildingCountroller.UnselectBuilding(); }

                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            m_UnitController.ShiftClickSelectUnit(hit.transform.GetComponent<UnitManager>());
                        }
                        else
                        {
                            m_UnitController.ClickSelectUnit(hit.transform.GetComponent<UnitManager>());
                        }
                    }
                    else if (hit.collider.CompareTag("Building"))
                    {
                        if (hit.transform.GetComponent<BuildingManager>() == null) { return; }
                        if (m_UnitController.SelectUnitList.Count != 0) { m_UnitController.UnselectAll(); }

                        m_BuildingCountroller.ClickSelectBuilding(hit.transform.GetComponent<BuildingManager>());
                    }
                }
                else if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerEnemy))
                {

                }
                else if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerThird))
                {
                    if (m_BuildingCountroller.SelectingBuilding != null) { m_BuildingCountroller.UnselectBuilding(); }
                    if (m_UnitController.SelectUnitList.Count != 0) { m_UnitController.UnselectAll(); }

                    if (!Input.GetKey(KeyCode.LeftShift) && hit.collider.CompareTag("Mineral") || hit.collider.CompareTag("BespeneGas"))
                    {
                        
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit hit;

                Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.CompareTag("Ground"))
                {
                    if (keyInput == 1)
                    {
                        m_UnitController.AttackSelectedUnit(hit.point);
                    }
                    else if(keyInput == 2)
                    {
                        m_UnitController.GatheringSelectedUnit(hit.transform.gameObject);
                    }
                    else if(keyInput == 0)
                    {
                        m_UnitController.MoveSelectedUnit(hit.point);
                    }
                    
                }
            }
        }
    }
}
