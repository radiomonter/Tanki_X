namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class AssetAsyncLoaderSystem : ECSSystem
    {
        public void AttachAssetToEntity(Object data, string name, Entity entity)
        {
            ResourceDataComponent component = new ResourceDataComponent {
                Data = data,
                Name = name
            };
            entity.AddComponent(component);
        }

        [OnEventFire]
        public void CancelAssetBundleLoading(NodeRemoveEvent e, SingleNode<AssetRequestComponent> assetRequest, [JoinSelf] SingleNode<LoadAssetBundlesRequestComponent> loadAssetBundlesRequest)
        {
            assetRequest.Entity.RemoveComponent<LoadAssetBundlesRequestComponent>();
        }

        [OnEventFire]
        public void CancelAssetListLoading(NodeRemoveEvent e, SingleNode<AssetRequestComponent> assetRequest, [JoinSelf] SingleNode<AsyncLoadingAssetListComponent> loadAssetBundlesRequest)
        {
            assetRequest.Entity.RemoveComponent<AsyncLoadingAssetListComponent>();
        }

        [OnEventFire]
        public void CancelAssetLoading(NodeRemoveEvent e, SingleNode<AssetRequestComponent> assetRequest, [JoinSelf] SingleNode<AsyncLoadingAssetComponent> loadAssetBundlesRequest)
        {
            assetRequest.Entity.RemoveComponent<AsyncLoadingAssetComponent>();
        }

        [OnEventFire]
        public void CompleteLoadAssetFromBundle(UpdateEvent e, AsyncLoadingAssetNode loadingAsset, [JoinAll] SingleNode<AssetBundleDatabaseComponent> db)
        {
            LoadAssetFromBundleRequest request = loadingAsset.asyncLoadingAsset.Request;
            if (request.IsDone)
            {
                Object asset = request.Asset;
                loadingAsset.assetReference.Reference.SetReference(asset);
                loadingAsset.Entity.RemoveComponent<AsyncLoadingAssetComponent>();
                this.AttachAssetToEntity(asset, request.ObjectName, loadingAsset.Entity);
            }
        }

        private static AsyncLoadAssetFromBundleRequest CreateLoadAssetRequest(AssetReference assetReference, AssetBundleDatabaseComponent db, AssetBundlesLoadDataComponent assetBundlesLoadData)
        {
            AsyncLoadAssetFromBundleRequest request;
            AssetInfo assetInfo = db.AssetBundleDatabase.GetAssetInfo(assetReference.AssetGuid);
            try
            {
                request = new AsyncLoadAssetFromBundleRequest(assetBundlesLoadData.LoadedBundles[assetInfo.AssetBundleInfo], assetInfo.ObjectName, assetInfo.AssetType);
            }
            catch (KeyNotFoundException exception)
            {
                throw new Exception("Bundle not found in LoadedBundles: " + assetInfo.AssetBundleInfo.BundleName + ", ref=" + assetReference.AssetGuid, exception);
            }
            return request;
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

        [OnEventFire]
        public void ProcessAssetRequest(NodeAddedEvent e, [Combine] AssetRequestNode node, DatabaseNode db)
        {
            string assetGuid = node.assetReference.Reference.AssetGuid;
            AssetInfo assetInfo = db.assetBundleDatabase.AssetBundleDatabase.GetAssetInfo(assetGuid);
            Entity entity = node.Entity;
            if (db.assetStorage.Contains(assetGuid))
            {
                Object data = db.assetStorage.Get(assetGuid);
                this.AttachAssetToEntity(data, assetInfo.ObjectName, entity);
            }
            else
            {
                LoadAssetBundlesRequestComponent component = new LoadAssetBundlesRequestComponent {
                    LoadingPriority = node.assetRequest.Priority
                };
                node.Entity.AddComponent(component);
            }
        }

        [OnEventFire]
        public void StartLoadAsset(UpdateEvent e, AsyncLoadingAssetNode loadingAsset, [JoinAll] Optional<SingleNode<TanyaSleepComponent>> tanya)
        {
            if (!tanya.IsPresent() && !loadingAsset.asyncLoadingAsset.Request.IsStarted)
            {
                loadingAsset.asyncLoadingAsset.Request.StartExecute();
            }
        }

        [OnEventComplete]
        public void StartLoadAssetFromBundle(AssetBundlesLoadedEvent e, AssetDependenciesNode asset, [JoinAll] SingleNode<AssetBundleDatabaseComponent> db)
        {
            AsyncLoadingAssetComponent component = new AsyncLoadingAssetComponent {
                Request = CreateLoadAssetRequest(asset.assetReference.Reference, db.component, asset.assetBundlesLoadData)
            };
            asset.Entity.AddComponent(component);
            asset.Entity.RemoveComponentIfPresent<LoadAssetBundlesRequestComponent>();
        }

        public class AssetDependenciesNode : Node
        {
            public AssetReferenceComponent assetReference;
            public AssetBundlesLoadDataComponent assetBundlesLoadData;
            public AssetRequestComponent assetRequest;
            public LoadAssetBundlesRequestComponent loadAssetBundlesRequest;
        }

        public class AssetRequestNode : Node
        {
            public AssetReferenceComponent assetReference;
            public AssetRequestComponent assetRequest;
        }

        public class AsyncLoadingAssetNode : Node
        {
            public AssetReferenceComponent assetReference;
            public AsyncLoadingAssetComponent asyncLoadingAsset;
            public AssetRequestComponent assetRequest;
        }

        public class DatabaseNode : Node
        {
            public AssetBundleDatabaseComponent assetBundleDatabase;
            public AssetStorageComponent assetStorage;
        }
    }
}

