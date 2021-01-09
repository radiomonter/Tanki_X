namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class NodeCollectorImpl : NodeCollector
    {
        private readonly Dictionary<NodeDescription, HashSet<Entity>> entitiesByDescription = new Dictionary<NodeDescription, HashSet<Entity>>();

        public void Attach(Entity entity, NodeDescription nodeDescription)
        {
            HashSet<Entity> set;
            if (!this.entitiesByDescription.TryGetValue(nodeDescription, out set))
            {
                set = new HashSet<Entity>();
                this.entitiesByDescription.Add(nodeDescription, set);
            }
            set.Add(entity);
        }

        public void Detach(Entity entity, NodeDescription nodeDescription)
        {
            this.entitiesByDescription[nodeDescription].Remove(entity);
        }

        public ICollection<Entity> FilterEntities(ICollection<Entity> values, NodeDescription nodeDescription)
        {
            HashSet<Entity> set;
            if (nodeDescription.IsEmpty)
            {
                List<Entity> list = Cache.listEntity.GetInstance();
                Collections.Enumerator<Entity> enumerator = Collections.GetEnumerator<Entity>(values);
                while (enumerator.MoveNext())
                {
                    Entity current = enumerator.Current;
                    if (((EntityInternal) current).Alive)
                    {
                        list.Add(current);
                    }
                }
                return list;
            }
            if (values.Count == 1)
            {
                Entity onlyElement = Collections.GetOnlyElement<Entity>(values);
                if (((EntityInternal) onlyElement).Contains(nodeDescription))
                {
                    return Collections.SingletonList<Entity>(onlyElement);
                }
            }
            if (!this.entitiesByDescription.TryGetValue(nodeDescription, out set))
            {
                return Collections.EmptyList<Entity>();
            }
            List<Entity> instance = Cache.listEntity.GetInstance();
            Collections.Enumerator<Entity> enumerator2 = Collections.GetEnumerator<Entity>(values);
            while (enumerator2.MoveNext())
            {
                Entity current = enumerator2.Current;
                if (set.Contains(current))
                {
                    instance.Add(current);
                }
            }
            return instance;
        }

        public ICollection<Entity> GetEntities(NodeDescription nodeDescription)
        {
            HashSet<Entity> set;
            if (nodeDescription.IsEmpty)
            {
                throw new EmptyNodeNotSupportedException();
            }
            return (!this.entitiesByDescription.TryGetValue(nodeDescription, out set) ? ((ICollection<Entity>) Collections.EmptyList<Entity>()) : ((ICollection<Entity>) set));
        }

        [Inject]
        public static FlowInstancesCache Cache { get; set; }
    }
}

