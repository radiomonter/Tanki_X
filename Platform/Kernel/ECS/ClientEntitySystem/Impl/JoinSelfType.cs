namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;

    public class JoinSelfType : JoinType
    {
        public ICollection<Entity> GetEntities(NodeCollectorImpl nodeCollector, NodeDescription nodeDescription, Entity key) => 
            !(key as EntityInternal).Contains(nodeDescription) ? Collections.EmptyList<Entity>() : Collections.SingletonList<Entity>(key);

        public Optional<Type> ContextComponent =>
            Optional<Type>.empty();
    }
}

