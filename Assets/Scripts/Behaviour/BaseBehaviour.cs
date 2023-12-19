using System;
using Consts;
using Helpers;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour
{
    public class BaseBehaviour : MonoBehaviourPun
    {
        public GameObject optionsUI;
        private SoldierFactory _soldierFactory;

        private Camera _camera;
        //public PlayerManager playerManager;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Awake()
        {
            _soldierFactory = GetComponent<SoldierFactory>();
        }
        private void Update()
        {
            if(!photonView.IsMine) return;
            if (!gameObject.IsMine())
            {
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                if(EventSystem.current.IsPointerOverGameObject())
                    return;
                
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject != gameObject)
                    {
                        optionsUI.SetActive(false);
                    }
                    else
                    {
                        optionsUI.SetActive(true);
                    }
                } 
            }
        }

        public void SpawnUnit(int unitType)
        {
            SoldierEnum soldierType = (SoldierEnum) unitType;
            
            var soldier = _soldierFactory.SpawnSoldier(soldierType); // ?? soldier
        }
    }
}