using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDragBehaviour : MonoBehaviour
{
    [SerializeField] private RectTransform dragRectangle;

    private Rect dragRect;
    private Vector2 start = Vector2.zero;
    private Vector2 end = Vector2.zero;

    private Camera mainCamera;
    private RTSUnitManager rtsUnitManager;

    private void Awake()
    {
        mainCamera = Camera.main;
        rtsUnitManager = GetComponent<RTSUnitManager>();

        DrawDragRectangle();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            start = Input.mousePosition;
            dragRect = new Rect();
        }

        if (Input.GetMouseButton(0))
        {
            end = Input.mousePosition;
            DrawDragRectangle();
        }

        if (Input.GetMouseButtonUp(0))
        {
            CalculateDragRect();
            SelectUnits();

            start = end = Vector2.zero;
            DrawDragRectangle();
        }
    }

    private void DrawDragRectangle()
    {
        dragRectangle.position = (start + end) * 0.5f;
        dragRectangle.sizeDelta = new Vector2(Mathf.Abs(start.x - end.x), Mathf.Abs(start.y - end.y));
    }

    private void CalculateDragRect()
    {
        if (Input.mousePosition.x < start.x)
        {
            dragRect.xMin = Input.mousePosition.x;
            dragRect.xMax = start.x;
        }
        else
        {
            dragRect.xMin = start.x;
            dragRect.xMax = Input.mousePosition.x;
        }

        if (Input.mousePosition.y < start.y)
        {
            dragRect.yMin = Input.mousePosition.y;
            dragRect.yMax = start.y;
        }
        else
        {
            dragRect.yMin = start.y;
            dragRect.yMax = Input.mousePosition.y;
        }
    }

    private void SelectUnits()
    {
        foreach (var unit in rtsUnitManager.UnitList)
            if (dragRect.Contains(mainCamera.WorldToScreenPoint(unit.transform.position)))
                rtsUnitManager.DragSelectUnit(unit);
    }
}