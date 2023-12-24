using System;
using System.Threading.Tasks;
using Consts;
using Helpers;
using Photon.Pun;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviourPun
    {
        public static GameManager instance;
        public GameObject spawner_green;
        public GameObject spawner_red;
        public GameObject base_green;
        public GameObject base_red;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            WaitingConnection();
        }

        private void Update()
        {
            
        }
        
        private void WaitingConnection()
        {
            if (MultiplayerHelper.Players.Count == 2)
            {
                photonView.RPC("AllConnected", RpcTarget.All);
                StartGame();
                return;
            }
            
            PopUp.Warning("Waiting for other player to connect", WaitingConnection);
        }

        [PunRPC]
        private void AllConnected()
        {
            PopUp.Success("All players connected, starting the game");
        }

        private async Task StartGame()
        {
            spawner_green.AssignOwner(MultiplayerHelper.MasterPlayer);
            base_green.AssignOwner(MultiplayerHelper.MasterPlayer);
            spawner_red.AssignOwner(MultiplayerHelper.NonMasterPlayer);
            base_red.AssignOwner(MultiplayerHelper.NonMasterPlayer);

            await Task.Delay(1000);
            MachineryBehaviour.greenMachine.gameObject.AssignOwner(MultiplayerHelper.MasterPlayer);
            MachineryBehaviour.redMachine.gameObject.AssignOwner(MultiplayerHelper.NonMasterPlayer);
            await Task.Delay(1500);
            PopUp.Success("Welcome to the Exoterra planet!");
        }
    }
}