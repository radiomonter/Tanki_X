namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    public interface ComponentServerChangeListener
    {
        void ChangedOnServer(Entity entity);
    }
}

