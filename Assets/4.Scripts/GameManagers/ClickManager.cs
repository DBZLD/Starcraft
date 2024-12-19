using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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

    public int keyInput;
    public ClickMod clickMod;

    private void Awake()
    {
        MainCamera = Camera.main;
        keyInput = 0;
        clickMod = (int)ClickMod.Normal;

        m_UnitController = GetComponent<UnitController>();
        m_BuildingCountroller = GetComponent<BuildingController>();
        m_MaterialController = GetComponent<MaterialController>();
        m_EnemyController = GetComponent<EnemyController>();
    }

    private void Update()
    {
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
                    else if(!Input.GetKey(KeyCode.LeftShift) && hit.collider.CompareTag("Ground"))
                    {
                        UnSelectAllObject("None");
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
               
                if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerAlly))
                {
                    if(hit.collider.CompareTag("Unit"))
                    {
                        m_UnitController.MoveSelectedUnit(hit.transform.gameObject);
                    }
                    else if(hit.collider.CompareTag("Building"))
                    {
                        m_UnitController.MoveSelectedUnit(hit.transform.gameObject);
                    }
                    
                }
                else if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerEnemy))
                {
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        m_UnitController.AttackSelectedUnit(hit.transform.gameObject);
                    }

                }
                else if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerThird))
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
                        else if (Input.GetKey(KeyCode.P))
                        {
                            m_UnitController.PatrolSelectedUnit(hit.point);
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
            }
        }
    }

    public void UnSelectAllObject(string exclusionScript)
    {
        if (exclusionScript == "None")
        {
            if (m_UnitController.selectUnitList.Count != 0) { m_UnitController.UnselectAll(); }
            if (m_MaterialController.selectMaterial != null) { m_MaterialController.UnselectMaterial(); }
            if (m_BuildingCountroller.selectBuilding != null) { m_BuildingCountroller.UnselectBuilding(); }
            if (m_EnemyController.selectEnemy != null) { m_EnemyController.UnselectEnemy(); }
        }
        else if (m_UnitController.selectUnitList.Count != 0 && exclusionScript != "Unit") { m_UnitController.UnselectAll(); }
        else if (m_MaterialController.selectMaterial != null && exclusionScript != "Material") { m_MaterialController.UnselectMaterial(); }
        else if (m_BuildingCountroller.selectBuilding != null && exclusionScript != "Building") { m_BuildingCountroller.UnselectBuilding(); }
        else if (m_EnemyController.selectEnemy != null && exclusionScript != "Enemy") { m_EnemyController.UnselectEnemy(); }

    }
}
