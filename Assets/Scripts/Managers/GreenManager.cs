using System;
using Consts;
using Managers.Abstract;
using Photon.Pun;
using UnityEngine;

namespace Managers
{
    public sealed class GreenManager : PlayerManager
    {
        [SerializeField] private SpriteRenderer base_health_bar;
        [SerializeField] private SpriteRenderer spawner_health_bar;

        private void Awake()
        {
            health_base = 1500f;
            health_spawner = 1500f;
            team = UnitTeam.Green;
            green_manager = this;
        }

        private void Update()
        {
            health_base = Mathf.Clamp(health_base, 0f, 1500f);

            var scaleX = health_base / 1500f;

            base_health_bar.transform.localScale = new Vector3(scaleX, base_health_bar.transform.localScale.y,
                base_health_bar.transform.localScale.z);

            health_spawner = Mathf.Clamp(health_spawner, 0f, 1500f);

            scaleX = health_spawner / 1500f;

            spawner_health_bar.transform.localScale = new Vector3(scaleX, spawner_health_bar.transform.localScale.y,
                spawner_health_bar.transform.localScale.z);
        }

        [PunRPC]
        public override void TakeDamage(float damage)
        {
            Debug.Log("taking damage" + health_base + " " + health_spawner);

            if (health_base > 0)
                health_base -= damage;
            else
                health_spawner -= damage;

            if (health_base < 0) GameObject.Find("Base_0").SetActive(false);

            if (health_spawner < 0)
            {
                GameObject.Find("Spawner_0").SetActive(false);
                GameOverBehaviour.GameOver(UnitTeam.Red, UnitTeam.Red,
                    green_manager.killCount
                );
            }
        }
    }
}