using System.Collections;
using System.Collections.Generic;
using Helpers;
using Photon.Pun;
using UnityEngine;

public class MouseClickBehaviour : MonoBehaviourPun
{
    [SerializeField] private LayerMask layerUnit;
    [SerializeField] private LayerMask layerGround;

    private Camera mainCamera;
    private RTSUnitManager rtsUnitManager;

    private void Awake()
    {
        mainCamera = Camera.main;
        rtsUnitManager = GetComponent<RTSUnitManager>();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerUnit))
            {
                var unitManager = hit.transform.GetComponent<UnitManager>();
                if (unitManager == null) return;

                if (Input.GetKey(KeyCode.LeftShift))
                    rtsUnitManager.ShiftClickSelectUnit(unitManager);
                else
                    rtsUnitManager.ClickSelectUnit(unitManager);
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift)) rtsUnitManager.DeselectAll();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerGround))
                rtsUnitManager.MoveSelectedUnits(hit.point);
        }
    }
}