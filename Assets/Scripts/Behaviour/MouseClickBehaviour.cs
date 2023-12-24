using System.Collections;
using System.Collections.Generic;
using Consts;
using Helpers;
using Photon.Pun;
using UnityEngine;

public class MouseClickBehaviour : MonoBehaviour
{
    [SerializeField] private LayerMask layerUnit;
    [SerializeField] private LayerMask layerGround;

    private UnitTeam currentTeam;
    private Camera mainCamera;
    private RTSUnitManager rtsUnitManager;

    private void Awake()
    {
        currentTeam = MultiplayerHelper.MasterPlayer.IsLocal ? UnitTeam.Green : UnitTeam.Red;
        mainCamera = Camera.main;
        rtsUnitManager = GetComponent<RTSUnitManager>();
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
                    rtsUnitManager.ShiftClickSelectUnit(unitManager);
                else
                    rtsUnitManager.ClickSelectUnit(unitManager);
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift)) rtsUnitManager.DeselectAll(currentTeam);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerGround))
            {
                rtsUnitManager.MoveSelectedUnits(hit.point, currentTeam);
            } 
            else if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerUnit))
            {
                var unitManager = hit.transform.GetComponent<UnitManager>();
                if (unitManager == null)
                {
                    var machineryBehaviour = hit.transform.GetComponent<MachineryBehaviour>();
                    if(machineryBehaviour != null && machineryBehaviour.unitTeam != currentTeam)
                        rtsUnitManager.AttackTo(machineryBehaviour);
                }
                else
                {
                    if(unitManager.unitTeam != currentTeam)
                        rtsUnitManager.AttackTo(unitManager);
                }
            }
        }
    }
}