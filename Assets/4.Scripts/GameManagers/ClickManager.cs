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
                        if (m_MaterialController.SelectingMaterial != null) { m_MaterialController.UnselectMaterial(); }

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
                        if (m_MaterialController.SelectingMaterial != null) { m_MaterialController.UnselectMaterial(); }

                        m_BuildingCountroller.ClickSelectBuilding(hit.transform.GetComponent<BuildingManager>());
                    }
                }
                else if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerEnemy))
                {
                    if (m_MaterialController.SelectingMaterial != null) { m_MaterialController.UnselectMaterial(); }
                    if (m_BuildingCountroller.SelectingBuilding != null) { m_BuildingCountroller.UnselectBuilding(); }

                    m_UnitController.ClickSelectUnit(hit.transform.GetComponent<UnitManager>());
                }
                else if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerThird))
                {
                    if (m_BuildingCountroller.SelectingBuilding != null) { m_BuildingCountroller.UnselectBuilding(); }
                    if (m_UnitController.SelectUnitList.Count != 0) { m_UnitController.UnselectAll(); }
                    if(m_MaterialController.SelectingMaterial != null) { m_MaterialController.UnselectMaterial(); }

                    if (!Input.GetKey(KeyCode.LeftShift) && hit.collider.CompareTag("Mineral") || hit.collider.CompareTag("BespeneGas"))
                    {
                        m_MaterialController.ClickSelectMaterial(hit.transform.GetComponent<MaterialManager>());
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

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerThird))
                {
                    if(hit.collider.CompareTag("Ground"))
                    {
                        if (Input.GetKey(KeyCode.A))
                        {
                            m_UnitController.AttackSelectedUnit(hit.point);
                        }
                        else if (Input.GetKey(KeyCode.G))
                        {
                            m_UnitController.GatheringSelectedUnit(hit.transform.gameObject);
                        }
                        else
                        {
                            m_UnitController.MoveSelectedUnit(hit.point);
                        }
                    }
                    else if(hit.collider.CompareTag("Mineral") || hit.collider.CompareTag("BespeneGas"))
                    {
                        m_UnitController.GatheringSelectedUnit(hit.transform.gameObject);
                    }
                }
                else if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerAlly))
                {
                    m_UnitController.MoveSelectedUnit(hit.transform.gameObject);
                }
                else if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerEnemy))
                {
                    m_UnitController.AttackSelectedUnit(hit.transform.gameObject);
                }
            }
        }
    }
}
