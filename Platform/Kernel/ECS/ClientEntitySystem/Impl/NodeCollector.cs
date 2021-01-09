namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;

    public interface NodeCollector
    {
        void Attach(Entity entity, NodeDescription nodeDescription);
        void Detach(Entity entity, NodeDescription nodeDescription);
        ICollection<Entity> FilterEntities(ICollection<Entity> values, NodeDescription nodeDescription);
        ICollection<Entity> GetEntities(NodeDescription nodeDescription);
    }
}

