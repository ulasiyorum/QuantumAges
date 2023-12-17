using System;
using System.Threading.Tasks;
using Consts;
using Helpers;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GameManager = Managers.GameManager;

public class SoldierFactory : MonoBehaviourPun
{
    [SerializeField] Vector3 spawnPosition;
    [SerializeField] bool localPlayer;
    [SerializeField] Animation anim_doors1;
    [SerializeField] Animation anim_doors2;
    public GameObject soldierPrefab_robot;
    public GameObject soldierPrefab_shooter;
    public GameObject soldierPrefab_superSoldier;
    private Player owner; // Green => LocalPlayer

    private void Awake()
    {
        owner = localPlayer ? MultiplayerHelper.MasterPlayer : MultiplayerHelper.NonMasterPlayer;
    }

    private void Update()
    {

    }

    public async Task<GameObject> SpawnSoldier(SoldierEnum soldier)
    {
        anim_doors1.CrossFade("Structure_v3_open", 0.1f);
        anim_doors2.CrossFade("Structure_v3_open", 0.1f);
        var animLength = anim_doors1.GetClip("Structure_v3_open").length + 0.6f;
        
        await Task.Delay(TimeSpan.FromSeconds(animLength));
        var rotation = Quaternion.Euler(0, 0, 0);
        GameObject go = soldier switch
        {
            SoldierEnum.Robot => Instantiate(soldierPrefab_robot, spawnPosition, Quaternion.identity),
            SoldierEnum.Shooter => Instantiate(soldierPrefab_shooter, spawnPosition, Quaternion.identity),
            SoldierEnum.SuperSoldier => Instantiate(soldierPrefab_superSoldier, spawnPosition, Quaternion.identity),
            _ => throw new ArgumentOutOfRangeException()
        };
        anim_doors2.CrossFade("Structure_v3_close", 0.1f);
        anim_doors1.CrossFade("Structure_v3_close", 0.1f);
        go.transform.rotation = rotation;
        var unitComponent = go.GetComponent<UnitManager>();
        unitComponent.soldierType = soldier;
        go.AssignOwner(owner);
        unitComponent.unitTeam = owner.IsLocal ? UnitTeam.Green : UnitTeam.Red;
        return go;
    }
}
