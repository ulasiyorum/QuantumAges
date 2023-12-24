using System;
using Consts;
using UnityEngine;

namespace Managers.Abstract
{
    public abstract class PlayerManager : MonoBehaviour, IDamagable
    {
        private Guid id = Guid.NewGuid();
        public static RedManager red_manager;
        public static GreenManager green_manager;
        public UnitTeam team;
        public int killCount = 0;
        
        public int green_crystal_balance;
        public int blue_crystal_balance;

        public float health_spawner = 1500f;
        public float health_base = 1500f;

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

        public void GetKill()
        {
            killCount++;
        }

        public Guid GetId()
        {
            return id;
        }
        
        public void TakeDamage(float damage)
        {
            if(health_base > 0)
                health_base -= damage;
            else
                health_spawner -= damage;
        }
    }
}