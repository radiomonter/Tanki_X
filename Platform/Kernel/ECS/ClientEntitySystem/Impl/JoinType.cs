namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System.Collections.Generic;

    public interface JoinType
    {
        ICollection<Entity> GetEntities(NodeCollectorImpl nodeCollector, NodeDescription nodeDescription, Entity key);

        Optional<Type> ContextComponent { get; }
    }
}

