using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickBehaviour : MonoBehaviour
{
    [SerializeField] private LayerMask layerUnit;
    [SerializeField] private LayerMask layerGround;

    private Camera mainCamera;
    private RTSUnitManager rtsUnitController;

    private void Awake()
    {
        mainCamera = Camera.main;
        rtsUnitController = GetComponent<RTSUnitManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerUnit))
            {
                var unitManager = hit.transform.GetComponent<UnitManager>();
                if (unitManager == null) return;
                if (Input.GetKey(KeyCode.LeftShift))
                    rtsUnitController.ShiftClickSelectUnit(unitManager);
                else
                    rtsUnitController.ClickSelectUnit(unitManager);
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift)) rtsUnitController.DeselectAll();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerGround))
                rtsUnitController.MoveSelectedUnits(hit.point);
        }
    }
}