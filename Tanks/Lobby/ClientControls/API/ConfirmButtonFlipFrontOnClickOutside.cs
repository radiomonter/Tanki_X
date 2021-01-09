namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class ConfirmButtonFlipFrontOnClickOutside : ECSBehaviour, IPointerEnterHandler, IPointerExitHandler, Component, AttachToEntityListener, DetachFromEntityListener, IEventSystemHandler
    {
        [SerializeField]
        private ConfirmButtonComponent confirmButton;
        private bool inside;
        private Entity entity;

        public void AttachedToEntity(Entity entity)
        {
            this.entity = entity;
        }

        public void DetachedFromEntity(Entity entity)
        {
            this.entity = null;
        }

        private void FlipFront()
        {
            this.confirmButton.FlipFront();
            if (this.entity != null)
            {
                base.ScheduleEvent<ConfirmButtonClickOutsideEvent>(this.entity);
            }
        }

        private void OnGUI()
        {
            if (this.confirmButton.EnableOutsideClicking)
            {
                if ((Event.current.type == EventType.MouseUp) && !this.inside)
                {
                    this.FlipFront();
                }
                if ((Event.current.type == EventType.KeyDown) && !Mathf.Approximately(Input.GetAxis("Vertical"), 0f))
                {
                    this.FlipFront();
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.inside = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.inside = false;
        }
    }
}

