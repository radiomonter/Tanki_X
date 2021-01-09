namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;

    public class AssetLoadByEventSystem : ECSSystem
    {
        [OnEventFire]
        public void Complete(NodeAddedEvent e, LoaderWithDataNode loaderWithData)
        {
            Type resourceDataComponentType = loaderWithData.loadByEventRequest.ResourceDataComponentType;
            if (resourceDataComponentType != null)
            {
                Entity owner = loaderWithData.loadByEventRequest.Owner;
                bool flag = owner.HasComponent(resourceDataComponentType);
                base.Log.InfoFormat("Complete {0} hasComponent={1}", resourceDataComponentType, flag);
                if (!flag)
                {
                    ResourceDataComponent component = (ResourceDataComponent) owner.CreateNewComponentInstance(resourceDataComponentType);
                    component.Data = loaderWithData.resourceData.Data;
                    owner.AddComponent(component);
                }
            }
            if (loaderWithData.Entity.Alive)
            {
                base.DeleteEntity(loaderWithData.Entity);
            }
        }

        [OnEventFire]
        public void ProcessRequest(AssetRequestEvent e, Node any)
        {
            Entity entity = base.CreateEntity("AssetLoadByEventRequest");
            entity.AddComponent(new AssetReferenceComponent(new AssetReference(e.AssetGuid)));
            LoadByEventRequestComponent component = new LoadByEventRequestComponent {
                ResourceDataComponentType = e.ResourceDataComponentType,
                Owner = any.Entity
            };
            entity.AddComponent(component);
            AssetRequestComponent component2 = new AssetRequestComponent {
                Priority = e.Priority,
                AssetStoreLevel = e.StoreLevel
            };
            entity.AddComponent(component2);
            e.LoaderEntity = entity;
        }

        public class LoaderWithDataNode : Node
        {
            public LoadByEventRequestComponent loadByEventRequest;
            public ResourceDataComponent resourceData;
        }
    }
}

