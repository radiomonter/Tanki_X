namespace Platform.Library.ClientResources.Impl
{
    using Assets.platform.library.ClientResources.Scripts.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientResources.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class AssetBundlePreloadSystem : ECSSystem
    {
        private const string ASSET_PRIORITY_CONFIG = "clientlocal/clientresources/assetpriority";

        [OnEventFire]
        public void CancelPreload(NodeRemoveEvent e, SingleNode<PreloadAllResourcesComponent> preload, [JoinAll] ICollection<PreloadNode> loadingRequests)
        {
            foreach (PreloadNode node in loadingRequests)
            {
                base.DeleteEntity(node.Entity);
            }
        }

        [OnEventComplete]
        public void Complete(AssetBundlesLoadedEvent e, PreloadNode loadingRequest)
        {
            base.DeleteEntity(loadingRequest.Entity);
        }

        [OnEventComplete]
        public void CompletePreload(UpdateEvent e, SingleNode<PreloadAllResourcesComponent> preload, [JoinAll] ICollection<PreloadNode> loadingRequests)
        {
            if (loadingRequests.Count == 0)
            {
                preload.Entity.RemoveComponent<PreloadAllResourcesComponent>();
            }
        }

        private void CreateEntityForPreloadingBundles(AssetReferenceComponent assetReferenceComponent, int loadingPriority)
        {
            Entity entity = base.CreateEntity("PreloadBundles");
            entity.AddComponent(assetReferenceComponent);
            entity.AddComponent<PreloadComponent>();
            LoadAssetBundlesRequestComponent component = new LoadAssetBundlesRequestComponent {
                LoadingPriority = loadingPriority
            };
            entity.AddComponent(component);
        }

        private static List<string> GetPrioritizedAssetsConfigPathList() => 
            ConfigurationService.GetConfig("clientlocal/clientresources/assetpriority").GetChildNode("configPathCollection").ConvertTo<ConfigPathCollectionComponent>().Collection;

        [OnEventFire]
        public void StartPreload(NodeAddedEvent e, SingleNode<PreloadAllResourcesComponent> preload, [JoinAll] DataBaseNode db)
        {
            if (DiskCaching.Enabled)
            {
                AssetBundleDatabase assetBundleDatabase = db.assetBundleDatabase.AssetBundleDatabase;
                AssetBundleDiskCache assetBundleDiskCache = db.assetBundleDiskCache.AssetBundleDiskCache;
                List<string> prioritizedAssetsConfigPathList = GetPrioritizedAssetsConfigPathList();
                int num = 100 + prioritizedAssetsConfigPathList.Count;
                List<string> list2 = new List<string>();
                for (int i = 0; i < prioritizedAssetsConfigPathList.Count; i++)
                {
                    AssetReferenceComponent assetReferenceComponent = AssetReferenceComponent.createFromConfig(prioritizedAssetsConfigPathList[i]);
                    string assetGuid = assetReferenceComponent.Reference.AssetGuid;
                    list2.Add(assetGuid);
                    AssetBundleInfo assetBundleInfoByGuid = assetBundleDatabase.GetAssetBundleInfoByGuid(assetGuid);
                    if (!assetBundleDiskCache.IsCached(assetBundleInfoByGuid))
                    {
                        this.CreateEntityForPreloadingBundles(assetReferenceComponent, num - i);
                    }
                }
                foreach (string str2 in assetBundleDatabase.GetRootGuids())
                {
                    AssetBundleInfo assetBundleInfoByGuid = assetBundleDatabase.GetAssetBundleInfoByGuid(str2);
                    if (!list2.Contains(str2) && !assetBundleDiskCache.IsCached(assetBundleInfoByGuid))
                    {
                        AssetReferenceComponent assetReferenceComponent = new AssetReferenceComponent(new AssetReference(str2));
                        this.CreateEntityForPreloadingBundles(assetReferenceComponent, 0);
                    }
                }
            }
        }

        [Inject]
        public static Platform.System.Data.Statics.ClientConfigurator.API.ConfigurationService ConfigurationService { get; set; }

        public class DataBaseNode : Node
        {
            public AssetBundleDatabaseComponent assetBundleDatabase;
            public BaseUrlComponent baseUrl;
            public AssetBundleDiskCacheComponent assetBundleDiskCache;
        }

        public class PreloadNode : Node
        {
            public AssetReferenceComponent assetReference;
            public PreloadComponent preload;
            public AssetBundlesLoadDataComponent assetBundlesLoadData;
        }
    }
}

