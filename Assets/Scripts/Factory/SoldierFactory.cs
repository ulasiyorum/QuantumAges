using System;
using Consts;
using Helpers;
using Photon.Realtime;
using UnityEngine;
using GameManager = Managers.GameManager;

public class SoldierFactory : MonoBehaviour
{
    [SerializeField] Vector3 spawnPosition;
    [SerializeField] bool? localPlayer;
    
    public GameObject soldierPrefab_robot;
    public GameObject soldierPrefab_shooter;
    public GameObject soldierPrefab_superSoldier;
    private Player owner; // Green => LocalPlayer

    private void Awake()
    {
        if(localPlayer is null)
            throw new ArgumentNullException("localPlayer must be set before calling Awake()");
        
        owner = localPlayer.Value ? MultiplayerHelper.LocalPlayer : MultiplayerHelper.OtherPlayer;
    }

    public GameObject SpawnSoldier(SoldierEnum soldier)
    {
        GameObject go = soldier switch
        {
            SoldierEnum.Robot => Instantiate(soldierPrefab_robot, spawnPosition, Quaternion.identity),
            SoldierEnum.Shooter => Instantiate(soldierPrefab_shooter, spawnPosition, Quaternion.identity),
            SoldierEnum.SuperSoldier => Instantiate(soldierPrefab_superSoldier, spawnPosition, Quaternion.identity),
            _ => throw new ArgumentOutOfRangeException()
        };

        var unitComponent = go.AddComponent<UnitManager>();
        unitComponent.soldierType = soldier;
        go.AssignOwner(owner);
        
        unitComponent.unitTeam = owner.IsLocal ? UnitTeam.Green : UnitTeam.Red;
        unitComponent.unitMarker = GameManager.instance.GetUnitMarker(unitComponent.unitTeam);
        
        return go;
    }
}
