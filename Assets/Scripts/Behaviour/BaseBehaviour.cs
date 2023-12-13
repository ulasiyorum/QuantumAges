using Helpers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour
{
    public class BaseBehaviour : MonoBehaviour, IPointerDownHandler
    {
        public GameObject optionsUI;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!gameObject.IsMine()) return;
            Debug.Log("IsMine Works");
            // clear soldier selections
            optionsUI.SetActive(true);
            // set active false if anywhere else tapped
        }
    }
}