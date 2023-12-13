using System;
using Consts;
using Helpers;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public GameObject spawner_green;
        public GameObject spawner_red;
        public GameObject base_green;
        public GameObject base_red;
        
        public GameObject greenUnitMarker;
        public GameObject redUnitMarker;
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
                PopUp.Success("All players connected, starting the game");
                StartGame();
                return;
            }
            
            PopUp.Warning("Waiting for other player to connect", WaitingConnection);
        }

        private void StartGame()
        {
            spawner_green.AssignOwner(MultiplayerHelper.LocalPlayer);
            base_green.AssignOwner(MultiplayerHelper.LocalPlayer);
            spawner_red.AssignOwner(MultiplayerHelper.OtherPlayer);
            base_red.AssignOwner(MultiplayerHelper.OtherPlayer);
        }
        
        public GameObject GetUnitMarker(UnitTeam unitTeam)
        {
            return unitTeam switch
            {
                UnitTeam.Green => greenUnitMarker,
                UnitTeam.Red => redUnitMarker,
                _ => throw new ArgumentOutOfRangeException(nameof(unitTeam), unitTeam, null)
            };
        }
    }
}