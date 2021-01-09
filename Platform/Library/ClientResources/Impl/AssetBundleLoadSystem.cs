namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class AssetBundleLoadSystem : ECSSystem
    {
        private const int MAX_LOADING_CHANNELS_COUNT = 4;
        private readonly PriorityAssetBundleNodeComparer loadPriorityComparer = new PriorityAssetBundleNodeComparer();

        [OnEventFire]
        public void CancelBundlesRequest(NodeRemoveEvent e, SingleNode<LoadAssetBundlesRequestComponent> request)
        {
            CleanBundleRequest(request.Entity);
        }

        private static void CleanBundleRequest(Entity entity)
        {
            if (entity.HasComponent<AssetGroupComponent>())
            {
                entity.RemoveComponent<AssetGroupComponent>();
            }
            if (entity.HasComponent<ResourceLoadStatComponent>())
            {
                entity.RemoveComponent<ResourceLoadStatComponent>();
            }
            if (entity.HasComponent<AssetBundlesLoadDataComponent>())
            {
                entity.RemoveComponent<AssetBundlesLoadDataComponent>();
            }
        }

        private void CollectBundles(AssetInfo info, ICollection<AssetBundleInfo> dependencies)
        {
            foreach (AssetBundleInfo info2 in info.AssetBundleInfo.AllDependencies)
            {
                dependencies.Add(info2);
            }
            dependencies.Add(info.AssetBundleInfo);
        }

        private void CreateAssetBundleLoadingChannel()
        {
            Entity entity = base.CreateEntity("LoadingChannel");
            entity.AddComponent<LoadingChannelComponent>();
            entity.AddComponent<LoadingChannelIdleComponent>();
        }

        private void CreateLoadingChannels(Entity assetBundleDatabaseEntity)
        {
            AssetBundleLoadingChannelsCountComponent component = new AssetBundleLoadingChannelsCountComponent();
            int num = Math.Max(1, Math.Min(SystemInfo.processorCount - 1, 4));
            component.DefaultChannelsCount = num;
            component.ChannelsCount = num;
            assetBundleDatabaseEntity.AddComponent(component);
            for (int i = 0; i < num; i++)
            {
                this.CreateAssetBundleLoadingChannel();
            }
        }

        [OnEventComplete]
        public void HandleBundleLoadComplete(LoadCompleteEvent e, AssetBundleLoadingNode node, [JoinAll] AssetBundleDatabaseNode dbNode)
        {
            AssetBundleLoadingComponent assetBundleLoading = node.assetBundleLoading;
            AssetBundleDiskCache assetBundleDiskCache = dbNode.assetBundleDiskCache.AssetBundleDiskCache;
            AssetBundleInfo info = node.assetBundleLoading.Info;
            if (!info.IsCached)
            {
                this.LogDownloadInfoIfBundleIsBig(assetBundleLoading, AssetBundleNaming.GetAssetBundleUrl(assetBundleDiskCache.BaseUrl, info.Filename), info.Size);
                info.IsCached = true;
            }
            this.ReleaseLoadingChannel(node.Entity);
        }

        [OnEventFire]
        public void InitSystem(NodeAddedEvent e, AssetBundleDatabaseNode database)
        {
            List<AssetBundleInfo> allAssetBundles = database.assetBundleDatabase.AssetBundleDatabase.GetAllAssetBundles();
            for (int i = 0; i < allAssetBundles.Count; i++)
            {
                AssetBundleInfo info = allAssetBundles[i];
                info.IsCached = database.assetBundleDiskCache.AssetBundleDiskCache.IsCached(info);
            }
            this.CreateLoadingChannels(database.Entity);
        }

        private bool IsLoadingComplete(BundlesRequestNode bundle) => 
            (bundle.assetBundlesLoadData.BundlesToLoad.Count == 0) && (bundle.assetBundlesLoadData.LoadingBundles.Count == 0);

        private void LogDownloadInfoIfBundleIsBig(AssetBundleLoadingComponent assetBundleLoadingComponent, string url, int size)
        {
            if (size > 0xe4e1c0)
            {
                float num = Time.realtimeSinceStartup - assetBundleLoadingComponent.StartTime;
                base.Log.InfoFormat("AssetBundleDownloadInfo\n url: {0}\n loadingTimeInSec: {1}\n bytesDownloaded: {2}", url, num, size);
            }
        }

        [OnEventFire]
        public void MarkLoaded(LoadCompleteEvent e, PreparedLoaderNode node, [JoinBy(typeof(AssetGroupComponent))] BundlesRequestNode bundlesRequestNode)
        {
            string bundleName = node.assetBundleLoading.Info.BundleName;
            AssetBundle assetBundle = node.assetBundleLoading.AssetBundleDiskCacheRequest.AssetBundle;
            bundlesRequestNode.assetBundlesLoadData.LoadingBundles.Remove(node.assetBundleLoading.Info);
            bundlesRequestNode.assetBundlesLoadData.LoadedBundles.Add(node.assetBundleLoading.Info, assetBundle);
        }

        private void PrepareAssetBundlesRequest(Entity request, HashSet<AssetBundleInfo> bundleInfos)
        {
            request.AddComponent(new AssetGroupComponent(request));
            request.AddComponent<ResourceLoadStatComponent>();
            AssetBundlesLoadDataComponent component = new AssetBundlesLoadDataComponent {
                AllBundles = bundleInfos,
                BundlesToLoad = new List<AssetBundleInfo>(bundleInfos),
                LoadingBundles = new HashSet<AssetBundleInfo>(),
                LoadedBundles = new Dictionary<AssetBundleInfo, AssetBundle>()
            };
            request.AddComponent(component);
        }

        [OnEventFire]
        public void PrepareBundlesRequest(NodeAddedEvent e, LoadBundlesForAssetRequestNode request, [JoinAll] SingleNode<AssetBundleDatabaseComponent> assetBundleDatabase)
        {
            AssetInfo assetInfo = assetBundleDatabase.component.AssetBundleDatabase.GetAssetInfo(request.assetReference.Reference.AssetGuid);
            HashSet<AssetBundleInfo> dependencies = new HashSet<AssetBundleInfo>();
            this.CollectBundles(assetInfo, dependencies);
            this.PrepareAssetBundlesRequest(request.Entity, dependencies);
        }

        [OnEventFire, Mandatory]
        public void PrepareBundleToLoad(PrepareBundleToLoadEvent e, BundlesRequestNode bundlesRequest, [JoinAll] ICollection<IdleLoadingChannelNode> loadingChannels, [JoinAll] SingleNode<BaseUrlComponent> baseUrlNode)
        {
            foreach (IdleLoadingChannelNode node in loadingChannels)
            {
                AssetBundleInfo info = this.SelectDependBundleToLoad(bundlesRequest);
                if (info == null)
                {
                    break;
                }
                this.PrepareLoadingChannel(info, baseUrlNode.component.Url, node, bundlesRequest.Entity);
            }
        }

        private void PrepareLoadingChannel(AssetBundleInfo info, string baseUrl, IdleLoadingChannelNode channelNode, Entity ownerBundle)
        {
            AssetBundlesStorage.MarkLoading(info);
            Entity entity = channelNode.Entity;
            entity.RemoveComponent<LoadingChannelIdleComponent>();
            entity.AddComponent(new AssetBundleLoadingComponent(info));
            entity.AddComponent(new AssetGroupComponent(ownerBundle));
        }

        private void ReleaseLoadingChannel(Entity channelEntity)
        {
            channelEntity.RemoveComponent<AssetBundleLoadingComponent>();
            channelEntity.RemoveComponent<AssetGroupComponent>();
            channelEntity.AddComponent<LoadingChannelIdleComponent>();
        }

        private AssetBundleInfo SelectDependBundleToLoad(BundlesRequestNode bundlesRequest)
        {
            AssetBundlesLoadDataComponent assetBundlesLoadData = bundlesRequest.assetBundlesLoadData;
            AssetBundleInfo info = null;
            List<AssetBundleInfo> list = new List<AssetBundleInfo>();
            for (int i = 0; i < assetBundlesLoadData.BundlesToLoad.Count; i++)
            {
                AssetBundleInfo info2 = assetBundlesLoadData.BundlesToLoad[i];
                if (AssetBundlesStorage.IsStored(info2))
                {
                    list.Add(info2);
                }
                else if (!AssetBundlesStorage.IsLoading(info2))
                {
                    if (i < (assetBundlesLoadData.BundlesToLoad.Count - 1))
                    {
                        assetBundlesLoadData.BundlesToLoad.Remove(info2);
                        assetBundlesLoadData.LoadingBundles.Add(info2);
                        info = info2;
                    }
                    else if ((assetBundlesLoadData.BundlesToLoad.Count == 1) && (assetBundlesLoadData.LoadingBundles.Count == 0))
                    {
                        assetBundlesLoadData.BundlesToLoad.Remove(info2);
                        assetBundlesLoadData.LoadingBundles.Add(info2);
                        info = info2;
                    }
                    break;
                }
            }
            foreach (AssetBundleInfo info3 in list)
            {
                assetBundlesLoadData.BundlesToLoad.Remove(info3);
                Dictionary<AssetBundleInfo, AssetBundle> loadedBundles = assetBundlesLoadData.LoadedBundles;
                if (!loadedBundles.ContainsKey(info3))
                {
                    loadedBundles.Add(info3, AssetBundlesStorage.Get(info3));
                }
            }
            return info;
        }

        private List<BundlesRequestNode> Sort(ICollection<BundlesRequestNode> requestedBundles)
        {
            List<BundlesRequestNode> list = new List<BundlesRequestNode>(requestedBundles);
            list.Sort(this.loadPriorityComparer);
            return list;
        }

        private bool TryCompleteLoading(BundlesRequestNode bundlesRequest)
        {
            if (!bundlesRequest.assetBundlesLoadData.Loaded)
            {
                if (!this.IsLoadingComplete(bundlesRequest))
                {
                    return false;
                }
                bundlesRequest.assetBundlesLoadData.Loaded = true;
                base.ScheduleEvent<AssetBundlesLoadedEvent>(bundlesRequest);
            }
            return true;
        }

        private void UpdateAssetBundleRequest(BundlesRequestNode bundlesRequest)
        {
            if (!this.TryCompleteLoading(bundlesRequest))
            {
                base.ScheduleEvent<PrepareBundleToLoadEvent>(bundlesRequest);
            }
        }

        private void UpdateBundleRequests(ICollection<BundlesRequestNode> bundlesRequestList)
        {
            if (bundlesRequestList.Count != 0)
            {
                List<BundlesRequestNode> list = this.Sort(bundlesRequestList);
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    BundlesRequestNode bundlesRequest = list[i];
                    this.UpdateAssetBundleRequest(bundlesRequest);
                }
            }
        }

        [OnEventFire]
        public void UpdateBundlesRequests(UpdateEvent e, SingleNode<BaseUrlComponent> baseUrlNode, [JoinAll] ICollection<BundlesRequestNode> bundlesRequestList)
        {
            this.UpdateBundleRequests(bundlesRequestList);
        }

        [OnEventFire]
        public void UpdateLoadingChannelsCount(UpdateEvent e, SingleNode<AssetBundleLoadingChannelsCountComponent> channelsCount, [JoinAll] ICollection<SingleNode<LoadingChannelComponent>> loadingChannels)
        {
            int num = channelsCount.component.ChannelsCount;
            int count = loadingChannels.Count;
            if (num != count)
            {
                if (num > count)
                {
                    for (int i = 0; i < (num - count); i++)
                    {
                        this.CreateAssetBundleLoadingChannel();
                    }
                }
                else
                {
                    foreach (SingleNode<LoadingChannelComponent> node in loadingChannels)
                    {
                        if (node.Entity.HasComponent<LoadingChannelIdleComponent>())
                        {
                            base.DeleteEntity(node.Entity);
                            if (num == (count - 1))
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        public class AssetBundleDatabaseNode : Node
        {
            public AssetBundleDatabaseComponent assetBundleDatabase;
            public BaseUrlComponent baseUrl;
            public AssetBundleDiskCacheComponent assetBundleDiskCache;
        }

        public class AssetBundleLoadingNode : Node
        {
            public AssetBundleLoadingComponent assetBundleLoading;
        }

        public class BundlesRequestNode : Node
        {
            public AssetGroupComponent assetGroup;
            public AssetBundlesLoadDataComponent assetBundlesLoadData;
            public LoadAssetBundlesRequestComponent loadAssetBundlesRequest;
        }

        public class IdleLoadingChannelNode : Node
        {
            public LoadingChannelComponent loadingChannel;
            public LoadingChannelIdleComponent loadingChannelIdle;
        }

        public class LoadBundlesForAssetRequestNode : Node
        {
            public AssetReferenceComponent assetReference;
            public LoadAssetBundlesRequestComponent loadAssetBundlesRequest;
        }

        public class PreparedLoaderNode : Node
        {
            public AssetBundleLoadingComponent assetBundleLoading;
            public AssetGroupComponent assetGroup;
        }

        private class PriorityAssetBundleNodeComparer : IComparer<AssetBundleLoadSystem.BundlesRequestNode>
        {
            public int Compare(AssetBundleLoadSystem.BundlesRequestNode x, AssetBundleLoadSystem.BundlesRequestNode y) => 
                Comparer<int>.Default.Compare(x.loadAssetBundlesRequest.LoadingPriority, y.loadAssetBundlesRequest.LoadingPriority);
        }
    }
}

