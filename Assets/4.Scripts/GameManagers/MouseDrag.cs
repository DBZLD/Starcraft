using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseDrag : MonoBehaviour
{
    [SerializeField] private RectTransform DragRectangle;

    private Rect DragRect;
    private Vector2 start = Vector2.zero;
    private Vector2 end = Vector2.zero;
    private bool isDrag = true;

    private Camera mainCamera;
    private UnitController m_UnitController;
    private BuildingController m_BuildingController;
    private MaterialController m_MaterialController;
    private EnemyController m_EnemyController;
    private void Awake()
    {
        mainCamera = Camera.main;
        m_UnitController = GetComponent<UnitController>();
        m_BuildingController = GetComponent<BuildingController>();
        m_MaterialController = GetComponent<MaterialController>();
        m_EnemyController = GetComponent<EnemyController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            if (!EventSystem.current.IsPointerOverGameObject() && m_BuildingController.selectBuilding == null && m_MaterialController.selectMaterial == null
               && m_EnemyController.selectEnemy == null && (hit.collider.CompareTag("Unit") || hit.collider.CompareTag("Ground")))
            {
                start = Input.mousePosition;
                DragRect = new Rect();
                isDrag = true;
            }
            else { isDrag = false; }
        }
        if (Input.GetMouseButton(0))
        {
            if (isDrag)
            {
                end = Input.mousePosition;

                DrawDragRectangle();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isDrag)
            {
                CalculateDragRect();
                SelectUnit();

                start = end = Vector2.zero;
                DrawDragRectangle();
            }
        }
    }

    private void DrawDragRectangle()
    {
        DragRectangle.position = (start + end) * 0.5f;
        DragRectangle.sizeDelta = new Vector2(Mathf.Abs(start.x - end.x), Mathf.Abs(start.y - end.y));
    }
    private void CalculateDragRect()
    {
        if (Input.mousePosition.x < start.x)
        {
            DragRect.xMin = Input.mousePosition.x;
            DragRect.xMax = start.x;
        }
        else
        {
            DragRect.xMin = start.x;
            DragRect.xMax = Input.mousePosition.x;
        }
        if (Input.mousePosition.y < start.y)
        {
            DragRect.yMin = Input.mousePosition.y;
            DragRect.yMax = start.y;
        }
        else
        {
            DragRect.yMin = start.y;
            DragRect.yMax = Input.mousePosition.y;
        }
    }
    private void SelectUnit()
    {
        foreach (UnitManager unit in m_UnitController.allUnitList)
        {
            if (DragRect.Contains(mainCamera.WorldToScreenPoint(unit.transform.position)))
            {
                m_UnitController.DragSelectUnit(unit);
            }
        }
    }
}
