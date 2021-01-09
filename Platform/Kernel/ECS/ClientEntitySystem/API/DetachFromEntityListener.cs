namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    public interface DetachFromEntityListener
    {
        void DetachedFromEntity(Entity entity);
    }
}

