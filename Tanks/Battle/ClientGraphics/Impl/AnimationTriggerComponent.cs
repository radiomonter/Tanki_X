namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;

    public abstract class AnimationTriggerComponent : ECSBehaviour, Component
    {
        private Platform.Kernel.ECS.ClientEntitySystem.API.Entity entity;

        protected AnimationTriggerComponent()
        {
        }

        protected void AddComponent<T>() where T: Component, new()
        {
            if (base.enabled)
            {
                this.AddComponentIfNeeded<T>();
            }
        }

        private void AddComponentIfNeeded<T>() where T: Component, new()
        {
            if (!this.Entity.HasComponent<T>())
            {
                this.Entity.AddComponent<T>();
            }
        }

        private void Awake()
        {
            base.enabled = false;
        }

        protected void ProvideEvent<T>() where T: Event, new()
        {
            if (base.enabled)
            {
                this.SendEvent<T>();
            }
        }

        protected void RemoveComponent<T>() where T: Component, new()
        {
            if (base.enabled)
            {
                this.RemoveComponentIfNeeded<T>();
            }
        }

        private void RemoveComponentIfNeeded<T>() where T: Component, new()
        {
            if (this.Entity.HasComponent<T>())
            {
                this.Entity.RemoveComponent<T>();
            }
        }

        private void SendEvent<T>() where T: Event, new()
        {
            base.NewEvent<T>().Attach(this.Entity).Schedule();
        }

        public Platform.Kernel.ECS.ClientEntitySystem.API.Entity Entity
        {
            get => 
                this.entity;
            set
            {
                this.entity = value;
                base.enabled = true;
            }
        }
    }
}

