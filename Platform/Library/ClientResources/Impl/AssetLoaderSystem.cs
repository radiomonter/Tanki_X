namespace Platform.Library.ClientResources.Impl
{
    using log4net;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientLogger.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class AssetLoaderSystem : ECSSystem
    {
        private ILog log;

        public void AttachResourceToEntity(Object data, string name, Entity entity)
        {
            ResourceDataComponent component = new ResourceDataComponent {
                Data = data,
                Name = name
            };
            entity.AddComponent(component);
        }

        private void CleanLoadingRequest(Entity entity)
        {
            if (entity.HasComponent<AssetBundlesLoadDataComponent>())
            {
                entity.RemoveComponent<AssetBundlesLoadDataComponent>();
            }
            if (entity.HasComponent<AssetGroupComponent>())
            {
                entity.RemoveComponent<AssetGroupComponent>();
            }
            if (entity.HasComponent<ResourceLoadStatComponent>())
            {
                entity.RemoveComponent<ResourceLoadStatComponent>();
            }
        }

        private void CollectBundles(AssetInfo info, ICollection<AssetBundleInfo> dependencies)
        {
            dependencies.Add(info.AssetBundleInfo);
            foreach (AssetBundleInfo info2 in info.AssetBundleInfo.AllDependencies)
            {
                dependencies.Add(info2);
            }
        }

        [OnEventComplete]
        public void CompleteLoadAssetFromBundle(AssetBundlesLoadedEvent e, AssetDependenciesNode assetNode, [JoinAll] SingleNode<AssetBundleDatabaseComponent> db)
        {
            AssetInfo assetInfo = db.component.AssetBundleDatabase.GetAssetInfo(assetNode.assetReference.Reference.AssetGuid);
            Entity entity = assetNode.Entity;
            Object data = this.ResolveAsset(assetInfo, assetNode.assetBundlesLoadData.LoadedBundles);
            this.AttachResourceToEntity(data, assetInfo.ObjectName, entity);
            assetNode.assetReference.Reference.SetReference(data);
            this.CleanLoadingRequest(assetNode.Entity);
        }

        private bool FillAllAssetsFromStorage(IEnumerable<AssetReference> referencies, AssetStorageComponent storage, out List<Object> assetList)
        {
            bool flag;
            assetList = new List<Object>(5);
            using (IEnumerator<AssetReference> enumerator = referencies.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        AssetReference current = enumerator.Current;
                        if (storage.Contains(current.AssetGuid))
                        {
                            assetList.Add(storage.Get(current.AssetGuid));
                            continue;
                        }
                        flag = false;
                    }
                    else
                    {
                        return true;
                    }
                    break;
                }
            }
            return flag;
        }

        private ILog GetLogger()
        {
            this.log ??= LoggerProvider.GetLogger(this);
            return this.log;
        }

        private void PrepareLoadingRequest(Entity request, HashSet<AssetBundleInfo> bundleInfos)
        {
            request.AddComponent(new AssetGroupComponent(request));
            AssetBundlesLoadDataComponent component = new AssetBundlesLoadDataComponent {
                AllBundles = bundleInfos,
                BundlesToLoad = new List<AssetBundleInfo>(bundleInfos),
                LoadingBundles = new HashSet<AssetBundleInfo>(),
                LoadedBundles = new Dictionary<AssetBundleInfo, AssetBundle>()
            };
            request.AddComponent(component);
            request.AddComponent<ResourceLoadStatComponent>();
        }

        [OnEventFire]
        public void ProcessAssetRequest(NodeAddedEvent e, [Combine] AssetRequestNode node, DatabaseNode db)
        {
            string assetGuid = node.assetReference.Reference.AssetGuid;
            AssetInfo assetInfo = db.assetBundleDatabase.AssetBundleDatabase.GetAssetInfo(assetGuid);
            Entity entity = node.Entity;
            if (db.assetStorage.Contains(assetGuid))
            {
                Object data = db.assetStorage.Get(assetGuid);
                this.AttachResourceToEntity(data, assetInfo.ObjectName, entity);
            }
            else
            {
                HashSet<AssetBundleInfo> dependencies = new HashSet<AssetBundleInfo>();
                this.CollectBundles(assetInfo, dependencies);
                this.PrepareLoadingRequest(entity, dependencies);
            }
        }

        private Object ResolveAsset(AssetInfo info, Dictionary<AssetBundleInfo, AssetBundle> cache)
        {
            AssetBundle bundle = cache[info.AssetBundleInfo];
            this.GetLogger().InfoFormat("LoadAsset objectName={0} objectType={1}", info.ObjectName, info.AssetType);
            return bundle.LoadAsset(info.ObjectName, info.AssetType);
        }

        [Not(typeof(PreloadComponent))]
        public class AssetDependenciesNode : Node
        {
            public AssetReferenceComponent assetReference;
            public AssetBundlesLoadDataComponent assetBundlesLoadData;
        }

        public class AssetRequestNode : Node
        {
            public AssetReferenceComponent assetReference;
            public AssetRequestComponent assetRequest;
        }

        public class DatabaseNode : Node
        {
            public AssetBundleDatabaseComponent assetBundleDatabase;
            public AssetStorageComponent assetStorage;
        }
    }
}

