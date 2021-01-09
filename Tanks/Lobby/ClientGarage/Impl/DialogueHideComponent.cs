namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class DialogueHideComponent : BehaviourComponent, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
    {
        public BaseDialogComponent dialogComponent;
        private bool pointerIn;

        private void OnDisable()
        {
            this.pointerIn = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.pointerIn = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.pointerIn = false;
        }

        private void Update()
        {
            if ((!this.pointerIn && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))) && (this.dialogComponent != null))
            {
                this.dialogComponent.Hide();
            }
        }
    }
}

