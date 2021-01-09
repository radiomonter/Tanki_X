namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public interface AutoRemoveComponentsRegistry
    {
        bool IsComponentAutoRemoved(Type componentType);
    }
}

