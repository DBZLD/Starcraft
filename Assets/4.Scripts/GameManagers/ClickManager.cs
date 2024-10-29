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
    private EnemyController m_EnemyController;

    public int keyInput = 0;

    private void Awake()
    {
        MainCamera = Camera.main;

        m_UnitController = GetComponent<UnitController>();
        m_BuildingCountroller = GetComponent<BuildingController>();
        m_MaterialController = GetComponent<MaterialController>();
        m_EnemyController = GetComponent<EnemyController>();
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
                        UnSelectAllObject("Unit");
                            m_UnitController.ClickSelectUnit(hit.transform.GetComponent<UnitManager>());
                    }
                    else if (hit.collider.CompareTag("Building"))
                    {
                        if (hit.transform.GetComponent<BuildingManager>() == null) { return; }
                        UnSelectAllObject("Building");

                        m_BuildingCountroller.ClickSelectBuilding(hit.transform.GetComponent<BuildingManager>());
                    }
                }
                else if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerEnemy))
                {
                    UnSelectAllObject("Enemy");

                    m_EnemyController.ClickSelectEnemy(hit.transform.GetComponent<EnemyManager>());
                }
                else if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerThird))
                {
                    UnSelectAllObject("Material");

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

    public void UnSelectAllObject(string exclusionScript)
    {
        if (m_UnitController.selectUnitList.Count != 0 && exclusionScript != "Unit") { m_UnitController.UnselectAll(); }
        if (m_MaterialController.selectMaterial != null && exclusionScript != "Material") { m_MaterialController.UnselectMaterial(); }
        if (m_BuildingCountroller.selectBuilding != null && exclusionScript != "Building") { m_BuildingCountroller.UnselectBuilding(); }
        if (m_EnemyController.selectEnemy != null && exclusionScript != "Enemy") { m_EnemyController.UnselectEnemy(); }
    }
}
