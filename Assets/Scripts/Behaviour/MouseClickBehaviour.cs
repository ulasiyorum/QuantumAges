using System.Collections;
using System.Collections.Generic;
using Consts;
using Helpers;
using Managers;
using Managers.Abstract;
using Photon.Pun;
using UnityEngine;

public class MouseClickBehaviour : MonoBehaviourPun
{
    [SerializeField] private LayerMask layerUnit;
    [SerializeField] private LayerMask layerGround;

    private UnitTeam currentTeam;
    private Camera mainCamera;
    private RTSUnitManager rtsUnitManager;

    private void Awake()
    {
        currentTeam = PhotonNetwork.IsMasterClient ? UnitTeam.Green : UnitTeam.Red;
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
                if (unitManager == null || unitManager.unitTeam != currentTeam) return;
                Debug.Log(currentTeam + " " + unitManager.unitTeam);
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
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerUnit))
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
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag("Base"))
                {
                    if (currentTeam != UnitTeam.Green)
                    {
                        if(PlayerManager.green_manager.health_base > 0)
                            rtsUnitManager.AttackTo(hit.point, currentTeam);
                        else
                            PopUp.Warning("Base is already destroyed");
                    }
                    else
                    {
                        if(PlayerManager.red_manager.health_base > 0)
                            rtsUnitManager.AttackTo(hit.point, currentTeam);
                        else
                            PopUp.Warning("Base is already destroyed");
                    }
                }

                if (hit.transform.CompareTag("Spawner"))
                {
                    if (currentTeam != UnitTeam.Green)
                    {
                        if (PlayerManager.green_manager.health_base > 0)
                        {
                            PopUp.Warning("You must destroy base first");
                            return;
                        }
                        
                        if(PlayerManager.green_manager.health_spawner > 0)
                            rtsUnitManager.AttackTo(hit.point, currentTeam);
                        else
                            GameOverBehaviour.GameOver(UnitTeam.Green, currentTeam, 
                                currentTeam == UnitTeam.Green ? PlayerManager.green_manager.killCount : PlayerManager.red_manager.killCount
                            );
                    }
                    
                    else
                    {
                        if (PlayerManager.red_manager.health_base > 0)
                        {
                            PopUp.Warning("You must destroy base first");
                            return;
                        }
                        
                        if(PlayerManager.red_manager.health_spawner > 0)
                            rtsUnitManager.AttackTo(hit.point, currentTeam);
                        else
                            GameOverBehaviour.GameOver(UnitTeam.Green, currentTeam, 
                                currentTeam == UnitTeam.Green ? PlayerManager.green_manager.killCount : PlayerManager.red_manager.killCount
                                );
                    }
                }
            }
        }
    }
}