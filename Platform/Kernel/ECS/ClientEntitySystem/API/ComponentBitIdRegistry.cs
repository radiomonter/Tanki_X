namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    public interface ComponentBitIdRegistry
    {
        int GetComponentBitId(Type componentClass);
    }
}

