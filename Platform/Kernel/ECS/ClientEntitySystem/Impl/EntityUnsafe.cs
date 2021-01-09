namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public interface EntityUnsafe
    {
        Component GetComponentUnsafe(Type componentType);
    }
}

