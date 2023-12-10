using System;
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
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            IsAllConnected();
        }

        private void Update()
        {
            
        }
        
        private bool IsAllConnected()
        {
            if (MultiplayerHelper.Players.Count == 2)
            {
                PopUp.Success("All players connected, starting the game");
                StartGame();
                return true;
            }
            
            PopUp.Warning("Waiting for other player to connect", () => IsAllConnected());
            return false;
        }

        private void StartGame()
        {
            Debug.Log("StartGame");
        }
    }
}