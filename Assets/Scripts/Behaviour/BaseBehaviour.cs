using System;
using System.Threading.Tasks;
using Consts;
using Helpers;
using Managers.Abstract;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour
{
    public class BaseBehaviour : MonoBehaviour
    {
        public GameObject optionsUI;
        private SoldierFactory _soldierFactory;

        private Camera _camera;

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
            if (_soldierFactory.owner?.UserId != PhotonNetwork.LocalPlayer.UserId)
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

        public async Task SpawnUnit(int unitType)
        {
            SoldierEnum soldierType = (SoldierEnum) unitType;

            var soldier = await _soldierFactory.SpawnSoldier(soldierType);
            
            if (soldier == null)
            {
                Debug.LogError("Soldier is null");
                return;
            }
        }
        
    }
}