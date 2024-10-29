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

    private void Awake()
    {
        mainCamera = Camera.main;
        m_UnitController = GetComponent<UnitController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
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
