namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;

    public class EntityTestImpl : EntityImpl, EntityTest, Entity
    {
        public EntityTestImpl(EngineServiceInternal engineService, long id, string name) : base(engineService, id, name)
        {
        }

        public EntityTestImpl(EngineServiceInternal engineService, long id, string name, Optional<TemplateAccessor> templateAccessor) : base(engineService, id, name, templateAccessor)
        {
        }

        public void AddComponentInTest<RealT>(Component component) where RealT: Component
        {
            Type comType = typeof(RealT);
            base.storage.AddComponentImmediately(comType, component);
            base.MakeNodes(comType, component);
            base.nodeAddedEventMaker.MakeIfNeed(this, comType);
        }

        public T GetComponentInTest<T>() where T: Component => 
            (T) base.GetComponent(typeof(T));

        public bool HasComponentInTest<T>() where T: Component
        {
            Type type = typeof(T);
            return base.HasComponent(type);
        }

        public void UpdateNodes()
        {
            BitSet componentsBitId = base.ComponentsBitId;
            foreach (NodeDescription description in ((NodeDescriptionRegistryImpl) NodeDescriptionRegistry).NodeDescriptions)
            {
                if (!base.nodeDescriptionStorage.Contains(description) && (componentsBitId.Mask(description.NodeComponentBitId) && componentsBitId.MaskNot(description.NotNodeComponentBitId)))
                {
                    base.AddNode(description);
                }
            }
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }
    }
}

