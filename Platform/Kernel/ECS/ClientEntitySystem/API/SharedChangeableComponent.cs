namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;

    public class SharedChangeableComponent : Component, AttachToEntityListener, DetachFromEntityListener
    {
        private EntityInternal entity;

        public void AttachedToEntity(Entity entity)
        {
            this.entity = (EntityInternal) entity;
        }

        public void DetachedFromEntity(Entity entity)
        {
            this.entity = null;
        }

        public void OnChange()
        {
            if ((this.entity != null) && this.entity.HasComponent(base.GetType()))
            {
                this.entity.NotifyComponentChange(base.GetType());
            }
        }
    }
}

