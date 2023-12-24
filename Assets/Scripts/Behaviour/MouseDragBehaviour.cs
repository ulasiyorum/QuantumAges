using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Consts;
using Photon.Pun;
using UnityEngine;

public class MouseDragBehaviour : MonoBehaviour
{
    [SerializeField] private RectTransform dragRectangle;

    private UnitTeam currentTeam;
    private Rect dragRect;
    private Vector2 start = Vector2.zero;
    private Vector2 end = Vector2.zero;
    private Camera mainCamera;
    private RectTransform mainCanvasRectTransform;
    private RTSUnitManager rtsUnitManager;
    
    private float WidthScaleFactor => mainCanvasRectTransform.sizeDelta.x / Screen.width;
    private float HeightScaleFactor => mainCanvasRectTransform.sizeDelta.y / Screen.height;
    private void Awake()
    {
        currentTeam = PhotonNetwork.IsMasterClient ? UnitTeam.Green : UnitTeam.Red;
        mainCanvasRectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
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
        dragRectangle.sizeDelta = new Vector2(Mathf.Abs(start.x * WidthScaleFactor - end.x * WidthScaleFactor), 
            Mathf.Abs(start.y * HeightScaleFactor - end.y * HeightScaleFactor));
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
        foreach (var unit in rtsUnitManager.UnitList.Where(x => x.unitTeam == currentTeam))
            if (dragRect.Contains(mainCamera.WorldToScreenPoint(unit.transform.position)))
                rtsUnitManager.DragSelectUnit(unit);
    }
}