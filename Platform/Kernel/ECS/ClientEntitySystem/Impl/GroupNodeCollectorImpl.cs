namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class GroupNodeCollectorImpl : NodeCollector
    {
        private readonly IDictionary<NodeDescription, ICollection<Entity>> entitiesByDescription = new Dictionary<NodeDescription, ICollection<Entity>>();

        public void Attach(Entity entity, NodeDescription nodeDescription)
        {
            ICollection<Entity> is2;
            if (!this.entitiesByDescription.TryGetValue(nodeDescription, out is2))
            {
                is2 = new HashSet<Entity>();
                this.entitiesByDescription[nodeDescription] = is2;
            }
            is2.Add(entity);
        }

        public void Detach(Entity entity, NodeDescription nodeDescription)
        {
            this.entitiesByDescription[nodeDescription].Remove(entity);
        }

        public ICollection<Entity> FilterEntities(ICollection<Entity> values, NodeDescription nodeDescription)
        {
            <FilterEntities>c__AnonStorey0 storey = new <FilterEntities>c__AnonStorey0 {
                entities = this.GetEntities(nodeDescription)
            };
            return values.Where<Entity>(new Func<Entity, bool>(storey.<>m__0)).ToList<Entity>();
        }

        public ICollection<Entity> GetEntities(NodeDescription nodeDescription)
        {
            ICollection<Entity> is2;
            if (nodeDescription.IsEmpty)
            {
                throw new EmptyNodeNotSupportedException();
            }
            return (!this.entitiesByDescription.TryGetValue(nodeDescription, out is2) ? Collections.EmptyList<Entity>() : is2);
        }

        [CompilerGenerated]
        private sealed class <FilterEntities>c__AnonStorey0
        {
            internal ICollection<Entity> entities;

            internal bool <>m__0(Entity value) => 
                this.entities.Contains(value);
        }
    }
}

