using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour
{
    [SerializeField] private LayerMask layerEnemy;
    [SerializeField] private LayerMask layerAlly;
    private Camera MainCamera;
    private UnitController m_UnitController;
    public int keyInput = 0;

    private void Awake()
    {
        MainCamera = Camera.main;

        m_UnitController = GetComponent<UnitController>();
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
                        if (hit.transform.GetComponent<UnitManager>() == null) return;

                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            m_UnitController.ShiftClickSelectUnit(hit.transform.GetComponent<UnitManager>());
                        }
                        else
                        {
                            m_UnitController.ClickSelectUnit(hit.transform.GetComponent<UnitManager>());
                        }
                    }
                    else if(hit.collider.CompareTag("Building"))
                    {
                        if (hit.transform.GetComponent<BuildingManager>() == null) return;

                        
                    }
                }
                if(Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.CompareTag("Ground"))
                {
                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        m_UnitController.UnselectAll();
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
