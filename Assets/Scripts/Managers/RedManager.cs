using System;
using Consts;
using Managers.Abstract;
using UnityEngine;

namespace Managers
{
    public sealed class RedManager : PlayerManager
    {
        [SerializeField] SpriteRenderer base_health_bar;
        [SerializeField] SpriteRenderer spawner_health_bar;
        private void Awake()
        {
            health_base = 1500f;
            health_spawner = 1500f;
            team = UnitTeam.Red;
            red_manager = this;
        }

        private void Update()
        {
            health_base = Mathf.Clamp(health_base, 0f, 1500f);
            float scaleX = health_base / 1500f;

            base_health_bar.transform.localScale = new Vector3(scaleX, base_health_bar.transform.localScale.y, base_health_bar.transform.localScale.z);
            
            health_spawner = Mathf.Clamp(health_spawner, 0f, 1500f);
            
            scaleX = health_spawner / 1500f;
            
            spawner_health_bar.transform.localScale = new Vector3(scaleX, spawner_health_bar.transform.localScale.y, spawner_health_bar.transform.localScale.z);
        }
    }
}