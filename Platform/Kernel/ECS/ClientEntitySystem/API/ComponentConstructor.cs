namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;

    public interface ComponentConstructor
    {
        Component GetComponentInstance(Type componentType, EntityInternal entity);
        bool IsAcceptable(Type componentType, EntityInternal entity);
    }
}

