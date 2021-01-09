namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;

    public class JoinAllType : JoinType
    {
        public ICollection<Entity> GetEntities(NodeCollectorImpl nodeCollector, NodeDescription nodeDescription, Entity key) => 
            nodeCollector.GetEntities(nodeDescription);

        public Optional<Type> ContextComponent =>
            Optional<Type>.empty();
    }
}

