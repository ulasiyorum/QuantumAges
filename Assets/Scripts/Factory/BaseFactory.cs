using Behaviour;
using Consts;
using Helpers;
using Photon.Pun;

namespace Factory
{
    public class BaseFactory : MonoBehaviourPun
    {
        public BaseBehaviour greenBase;
        public BaseBehaviour redBase;
        
        public void SpawnUnit(int unitType)
        {
            if (!photonView.IsMine) return;
            
            UnitTeam team = MultiplayerHelper.MasterPlayer.IsLocal ? UnitTeam.Green : UnitTeam.Red;
            
            switch (team)
            {
                case UnitTeam.Green:
                    greenBase.SpawnUnit(unitType);
                    break;
                case UnitTeam.Red:
                    redBase.SpawnUnit(unitType);
                    break;
            }
        }
    }
}