using UnityEngine;

namespace Managers.Abstract
{
    public abstract class PlayerManager : MonoBehaviour
    {
        public static RedManager red_manager;
        public static GreenManager green_manager;
        
        public int green_crystal_balance = 0;
        public int blue_crystal_balance = 0;
    }
}