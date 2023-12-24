using System.Collections;
using System.Collections.Generic;
using Consts;
using Helpers;
using Managers.Abstract;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class BalanceViewer : MonoBehaviourPun
{
    public ResourceType type;
    private TMP_Text text;
    private PlayerManager playerManager;
    void Start()
    {
        playerManager = MultiplayerHelper.MasterPlayer.IsLocal
            ? PlayerManager.green_manager
            : PlayerManager.red_manager;
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (type == ResourceType.GreenCrystal)
            text.text = playerManager.green_crystal_balance.ToString();
        
        else if (type == ResourceType.BlueCrystal)
            text.text = playerManager.blue_crystal_balance.ToString();
    }
}
