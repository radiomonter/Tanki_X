namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class AssetStorageSystem : ECSSystem
    {
        public static int MANAGED_RESOURCE_EXPIRE_DURATION_SEC = 300;

        [OnEventFire]
        public void Add(NodeAddedEvent e, SingleNode<AssetBundleDatabaseComponent> databaseNode)
        {
            databaseNode.Entity.AddComponent<AssetStorageComponent>();
        }

        [OnEventFire]
        public void CleanStorage(UnloadUnusedAssetsEvent e, Node any, [JoinAll] DatabaseNode db)
        {
            AssetStorageComponent assetStorage = db.assetStorage;
            List<string> list = new List<string>(10);
            foreach (KeyValuePair<string, ResourceStorageEntry> pair in db.assetStorage.ManagedReferencies)
            {
                ResourceStorageEntry entry = pair.Value;
                if (this.IsExpired(entry))
                {
                    list.Add(pair.Key);
                }
            }
            foreach (string str in list)
            {
                assetStorage.Remove(str, AssetStoreLevel.MANAGED);
            }
        }

        private bool IsExpired(ResourceStorageEntry entry) => 
            (entry.LastAccessTime + MANAGED_RESOURCE_EXPIRE_DURATION_SEC) > Time.time;

        [OnEventFire]
        public void Store(NodeAddedEvent e, LoadedAssetNode assetNode, [JoinAll] DatabaseNode db)
        {
            AssetStorageComponent assetStorage = db.assetStorage;
            string assetGuid = assetNode.assetReference.Reference.AssetGuid;
            Object data = assetNode.resourceData.Data;
            if (data != null)
            {
                assetStorage.Put(assetGuid, data, assetNode.assetRequest.AssetStoreLevel);
            }
        }

        public class DatabaseNode : Node
        {
            public AssetBundleDatabaseComponent assetBundleDatabase;
            public AssetStorageComponent assetStorage;
        }

        public class LoadedAssetNode : Node
        {
            public AssetRequestComponent assetRequest;
            public ResourceDataComponent resourceData;
            public AssetReferenceComponent assetReference;
        }
    }
}

