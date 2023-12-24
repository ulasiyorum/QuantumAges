using System;
using Consts;
using UnityEngine;

namespace Managers.Abstract
{
    public abstract class PlayerManager : MonoBehaviour
    {
        public static RedManager red_manager;
        public static GreenManager green_manager;

        public int green_crystal_balance;
        public int blue_crystal_balance;

        private void Start()
        {
            green_crystal_balance = 20;
            blue_crystal_balance = 20;
        }

        public bool Cashout(int money, ResourceType type)
        {
            if (type == ResourceType.GreenCrystal)
            {
                if (green_crystal_balance < money) return false;
                green_crystal_balance -= money;
            }
            else
            {
                if (blue_crystal_balance < money) return false;
                blue_crystal_balance -= money;
            }

            return true;
        }
    }
}