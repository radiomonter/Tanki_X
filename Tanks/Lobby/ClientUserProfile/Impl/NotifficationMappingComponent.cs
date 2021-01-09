namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class NotifficationMappingComponent : BehaviourComponent, IPointerClickHandler, AttachToEntityListener, DetachFromEntityListener, IEventSystemHandler
    {
        [SerializeField]
        private bool clickAnywhere;
        private Entity entity;

        private void MouseClicked()
        {
            ECSBehaviour.EngineService.Engine.ScheduleEvent<NotificationClickEvent>(this.entity);
            base.enabled = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            this.MouseClicked();
        }

        void AttachToEntityListener.AttachedToEntity(Entity entity)
        {
            this.entity = entity;
        }

        void DetachFromEntityListener.DetachedFromEntity(Entity entity)
        {
            this.entity = null;
        }

        private void Update()
        {
            if (this.clickAnywhere && Input.GetMouseButton(0))
            {
                this.MouseClicked();
            }
        }
    }
}

