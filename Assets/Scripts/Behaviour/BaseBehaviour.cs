using System;
using Helpers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour
{
    public class BaseBehaviour : MonoBehaviour
    {
        public GameObject optionsUI;
        private SoldierFactory _soldierFactory;

        private void Awake()
        {
            _soldierFactory = GetComponent<SoldierFactory>();
        }

        public void OnMouseDown()
        {
            if (!gameObject.IsMine())
            {
                Debug.Log("not mine");
                return;
            }
            Debug.Log("mine");
            // clear soldier selections
            //optionsUI.SetActive(true);
            // set active false if anywhere else tapped
        }
    }
}