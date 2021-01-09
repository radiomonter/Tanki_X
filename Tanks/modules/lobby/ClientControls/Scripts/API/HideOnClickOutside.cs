namespace tanks.modules.lobby.ClientControls.Scripts.API
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class HideOnClickOutside : UIBehaviour, IPointerExitHandler, IPointerEnterHandler, IEventSystemHandler
    {
        private bool hasFocus;

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.hasFocus = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.hasFocus = false;
        }

        private void Update()
        {
            if (!this.hasFocus && (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)))
            {
                base.gameObject.SetActive(false);
            }
        }
    }
}

