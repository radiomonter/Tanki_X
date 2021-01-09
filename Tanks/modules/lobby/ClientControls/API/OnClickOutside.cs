namespace tanks.modules.lobby.ClientControls.API
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;

    public class OnClickOutside : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
    {
        [SerializeField]
        private UnityEvent onClick;
        private bool isInside;

        private void OnDisable()
        {
            this.isInside = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.isInside = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.isInside = false;
        }

        private void Update()
        {
            if (!this.isInside && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
            {
                this.onClick.Invoke();
            }
        }
    }
}

