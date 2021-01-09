namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;

    public class JoinByType : JoinType
    {
        private readonly Type contextComponent;

        public JoinByType(Type contextComponent)
        {
            this.contextComponent = contextComponent;
        }

        public ICollection<Entity> GetEntities(NodeCollectorImpl nodeCollector, NodeDescription nodeDescription, Entity key)
        {
            GroupComponent component;
            return (((component = (GroupComponent) ((EntityUnsafe) key).GetComponentUnsafe(this.contextComponent)) == null) ? Collections.EmptyList<Entity>() : component.GetGroupMembers(nodeDescription));
        }

        public Optional<Type> ContextComponent =>
            Optional<Type>.of(this.contextComponent);
    }
}

