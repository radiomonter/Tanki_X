namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class NodeCache
    {
        private NodesToChange nodesToChange;

        public NodeCache(EngineServiceInternal engineService)
        {
        }

        protected ICollection<NodeDescription> GetAddedNodes(EntityInternal entity, Type componentClass)
        {
            BitSet componentsBitId = entity.ComponentsBitId;
            List<NodeDescription> instance = flowInstances.listNodeDescription.GetInstance();
            Collections.Enumerator<NodeDescription> enumerator = Collections.GetEnumerator<NodeDescription>(NodeDescriptionRegistry.GetNodeDescriptions(componentClass));
            while (enumerator.MoveNext())
            {
                NodeDescription current = enumerator.Current;
                if (componentsBitId.Mask(current.NodeComponentBitId) && componentsBitId.MaskNot(current.NotNodeComponentBitId))
                {
                    instance.Add(current);
                }
            }
            return instance;
        }

        public virtual NodesToChange GetNodesToChange(EntityInternal entity, Type componentClass) => 
            new NodesToChange { 
                NodesToAdd = this.GetAddedNodes(entity, componentClass),
                NodesToRemove = this.GetRemovedNodes(entity, componentClass)
            };

        protected ICollection<NodeDescription> GetRemovedNodes(EntityInternal entity, Type componentClass)
        {
            BitSet componentsBitId = entity.ComponentsBitId;
            List<NodeDescription> instance = flowInstances.listNodeDescription.GetInstance();
            Collections.Enumerator<NodeDescription> enumerator = Collections.GetEnumerator<NodeDescription>(NodeDescriptionRegistry.GetNodeDescriptionsByNotComponent(componentClass));
            while (enumerator.MoveNext())
            {
                NodeDescription current = enumerator.Current;
                if (componentsBitId.Mask(current.NodeComponentBitId) && (!componentsBitId.MaskNot(current.NotNodeComponentBitId) && entity.NodeDescriptionStorage.Contains(current)))
                {
                    instance.Add(current);
                }
            }
            return instance;
        }

        [Inject]
        public static FlowInstancesCache flowInstances { get; set; }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }
    }
}

