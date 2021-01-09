namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;

    public abstract class AbstractBackButtonComponent<T> : ECSBehaviour, Component, AttachToEntityListener, DetachFromEntityListener where T: Event, new()
    {
        private Entity entity;
        private bool disabled;

        protected AbstractBackButtonComponent()
        {
        }

        private void OnEnable()
        {
            this.disabled = false;
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
            if ((this.entity != null) && (!this.disabled && (!InputFieldComponent.IsAnyInputFieldInFocus() && InputMapping.Cancel)))
            {
                base.ScheduleEvent<T>(this.entity);
            }
        }

        public bool Disabled
        {
            get => 
                this.disabled;
            set => 
                this.disabled = value;
        }
    }
}

