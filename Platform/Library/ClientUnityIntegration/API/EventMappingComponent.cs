namespace Platform.Library.ClientUnityIntegration.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public abstract class EventMappingComponent : BehaviourComponent, AttachToEntityListener, DetachFromEntityListener
    {
        protected Entity entity;

        protected EventMappingComponent()
        {
        }

        public void AttachedToEntity(Entity entity)
        {
            this.entity = entity;
        }

        protected virtual void Awake()
        {
            this.Subscribe();
        }

        public void DetachedFromEntity(Entity entity)
        {
            this.entity = null;
        }

        protected virtual void SendEvent<T>() where T: Event, new()
        {
            if (this.entity != null)
            {
                ECSBehaviour.EngineService.Engine.ScheduleEvent<T>(this.entity);
            }
        }

        protected abstract void Subscribe();
    }
}

