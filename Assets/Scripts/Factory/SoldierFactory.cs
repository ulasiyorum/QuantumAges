using System;
using System.Threading.Tasks;
using Consts;
using Helpers;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
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
        owner = localPlayer ? MultiplayerHelper.LocalPlayer : MultiplayerHelper.OtherPlayer;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
            SpawnSoldier(SoldierEnum.Robot);
        if (Input.GetKeyDown(KeyCode.S))
            SpawnSoldier(SoldierEnum.Shooter);
        if (Input.GetKeyDown(KeyCode.D))
            SpawnSoldier(SoldierEnum.SuperSoldier);
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
            SoldierEnum.Robot => Instantiate(soldierPrefab_robot, spawnPosition, rotation),
            SoldierEnum.Shooter => Instantiate(soldierPrefab_shooter, spawnPosition, rotation),
            SoldierEnum.SuperSoldier => Instantiate(soldierPrefab_superSoldier, spawnPosition, rotation),
            _ => throw new ArgumentOutOfRangeException()
        };
        anim_doors2.CrossFade("Structure_v3_close", 0.1f);
        anim_doors1.CrossFade("Structure_v3_close", 0.1f);
        var unitComponent = go.AddComponent<UnitManager>();
        unitComponent.soldierType = soldier;
        go.AssignOwner(owner);
        
        unitComponent.unitTeam = owner.IsLocal ? UnitTeam.Green : UnitTeam.Red;
        unitComponent.unitMarker = GameManager.instance.GetUnitMarker(unitComponent.unitTeam);
        return go;
    }
}
