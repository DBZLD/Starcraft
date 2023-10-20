using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//ui클릭시 유닛 선택 및 이동 안됨
public class ClickManager : MonoBehaviour
{
    [SerializeField] private LayerMask layerUnit;
    [SerializeField] private LayerMask layerGround;

    private Camera MainCamera;
    private UnitController m_UnitController;

    private void Awake()
    {
        MainCamera = Camera.main;

        m_UnitController = GetComponent<UnitController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit hit;

                Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerUnit))
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
                else
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

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerGround))
                {
                    m_UnitController.MoveSelectedUnit(hit.point);
                }
            }
        }
    }
}
