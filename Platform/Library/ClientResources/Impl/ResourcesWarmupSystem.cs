namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ResourcesWarmupSystem : ECSSystem
    {
        private const int RESOURCE_WARMUP_COUNT_PER_FRAME = 3;
        [CompilerGenerated]
        private static Func<string, AssetReference> <>f__am$cache0;

        [OnEventFire]
        public void RequestWarmupResources(NodeAddedEvent e, WarmupResourcesNode node)
        {
            AssetRequestComponent component = new AssetRequestComponent {
                AssetStoreLevel = AssetStoreLevel.STATIC,
                Priority = 100
            };
            node.Entity.AddComponent(component);
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = guid => new AssetReference(guid);
            }
            List<AssetReference> list = node.warmupResources.AssetGuids.Select<string, AssetReference>(<>f__am$cache0).ToList<AssetReference>();
            node.Entity.AddComponent<ResourceWarmupIndexComponent>();
        }

        private void WarmInstanceComponents(GameObject instance)
        {
            WarmableResourceBehaviour[] componentsInChildren = instance.GetComponentsInChildren<WarmableResourceBehaviour>(true);
            int length = componentsInChildren.Length;
            for (int i = 0; i < length; i++)
            {
                componentsInChildren[i].WarmUp();
            }
        }

        public class WarmupResourcesNode : Node
        {
            public WarmupResourcesComponent warmupResources;
        }
    }
}

