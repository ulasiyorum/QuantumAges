using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Helpers
{
    public static class MultiplayerHelper
    {
        public static List<Player> Players => PhotonNetwork.PlayerList.ToList();
        public static Player LocalPlayer => PhotonNetwork.LocalPlayer;
        public static Player OtherPlayer => Players.FirstOrDefault(p => !p.Equals(LocalPlayer));
        
        public static void AssignOwner(this GameObject gameObject, Player player)
        {
            if(gameObject is null)
                throw new Exception("GameObject is null");
            
            if(gameObject.GetPhotonView().Owner.Equals(player)) return;
            
            gameObject.GetPhotonView().TransferOwnership(player);
        }
    }
}