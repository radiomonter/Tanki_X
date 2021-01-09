namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    public interface AttachToEntityListener
    {
        void AttachedToEntity(Entity entity);
    }
}

