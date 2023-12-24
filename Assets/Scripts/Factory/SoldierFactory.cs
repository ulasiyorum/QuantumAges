using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Consts;
using Helpers;
using Managers;
using Models;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;
using PlayerManager = Managers.Abstract.PlayerManager;

public class SoldierFactory : MonoBehaviourPun
{
    [SerializeField] Vector3 spawnPosition;
    [SerializeField] bool localPlayer;
    [SerializeField] Animation anim_doors1;
    [SerializeField] Animation anim_doors2;
    public GameObject soldierPrefab_robot;
    public GameObject soldierPrefab_shooter;
    public GameObject soldierPrefab_superSoldier;
    public Player owner; // Green => MasterPlayer
    
    public static List<PriceModel> _prices = new List<PriceModel>
    {
        new PriceModel(3, ResourceType.GreenCrystal, SoldierEnum.Robot),
        new PriceModel(7, ResourceType.GreenCrystal, SoldierEnum.Shooter),
        new PriceModel(2, ResourceType.BlueCrystal, SoldierEnum.SuperSoldier),
    };


    private bool testMode = false;
    private void Awake()
    {
        owner = localPlayer ? MultiplayerHelper.MasterPlayer : MultiplayerHelper.NonMasterPlayer;
    }

    private void Update()
    {
        #if UNITY_EDITOR

        if (!testMode) return;
        
        if (Input.GetKeyDown(KeyCode.J))
            SpawnSoldier(SoldierEnum.Robot);

        if (Input.GetKeyDown(KeyCode.L))
            SpawnSoldier(SoldierEnum.SuperSoldier);

        if (Input.GetKeyDown(KeyCode.K))
            SpawnSoldier(SoldierEnum.Shooter);
        #endif
    }

    
    public async Task<SpawnedSoldierModel> SpawnSoldier(SoldierEnum soldier)
    {
        PriceModel priceModel;
        bool result = false;
        var unitTeam = owner.IsMasterClient ? UnitTeam.Green : UnitTeam.Red;
        
        switch (unitTeam)
        {
            case UnitTeam.Green:
                priceModel = _prices.Find(x => x.SoldierType == soldier);

                if (priceModel is null)
                    throw new ArgumentOutOfRangeException($"{soldier.ToString()} is not set in PriceModel");

                result = PlayerManager.green_manager.Cashout(priceModel.Crystal, priceModel.ResourceType);

                if (!result)
                {
                    PopUp.Error("Not enough resources");
                }
                
                break;
            case UnitTeam.Red:
                priceModel = _prices.Find(x => x.SoldierType == soldier);

                if (priceModel is null)
                    throw new ArgumentOutOfRangeException($"{soldier.ToString()} is not set in PriceModel");

                result = PlayerManager.red_manager.Cashout(priceModel.Crystal, priceModel.ResourceType);

                if (!result)
                {
                    PopUp.Error("Not enough resources");
                }
                
                break;
            
            default:
                throw new ArgumentOutOfRangeException("Must be either red or green");
        }

        if (!result)
            return null;
        
        anim_doors1.CrossFade("Structure_v3_open", 0.1f);
        anim_doors2.CrossFade("Structure_v3_open", 0.1f);
        var animLength = anim_doors1.GetClip("Structure_v3_open").length + 0.6f;
        
        await Task.Delay(TimeSpan.FromSeconds(animLength));
        var rotation = Quaternion.Euler(0, 0, 0);
        GameObject go = soldier switch
        {
            SoldierEnum.Robot => PhotonNetwork.Instantiate(soldierPrefab_robot.name, spawnPosition, Quaternion.identity),
            SoldierEnum.Shooter => PhotonNetwork.Instantiate(soldierPrefab_shooter.name, spawnPosition, Quaternion.identity),
            SoldierEnum.SuperSoldier => PhotonNetwork.Instantiate(soldierPrefab_superSoldier.name, spawnPosition, Quaternion.identity),
            _ => throw new ArgumentOutOfRangeException()
        };
        anim_doors2.CrossFade("Structure_v3_close", 0.1f);
        anim_doors1.CrossFade("Structure_v3_close", 0.1f);
        var unitComponent = go.GetComponent<UnitManager>();

        go.transform.rotation = rotation;
        unitComponent.soldierType = soldier;
        go.AssignOwner(owner);
        unitComponent.photonView.RPC("SetUnitTeam", RpcTarget.All, (int)unitTeam);

        
        return new SpawnedSoldierModel
        {
            unitManager = unitComponent,
            spawnedSoldier = go
        };
    }
}
