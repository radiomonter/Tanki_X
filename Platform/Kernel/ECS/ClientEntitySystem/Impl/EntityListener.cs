namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public interface EntityListener
    {
        void OnEntityDeleted(Entity entity);
        void OnNodeAdded(Entity entity, NodeDescription nodeDescription);
        void OnNodeRemoved(Entity entity, NodeDescription nodeDescription);
    }
}

