namespace Tanks.Lobby.ClientLoading.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientResources.Impl;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Tanks.Lobby.ClientLoading.API;
    using UnityEngine;

    public class AssetBundleLoadingProgressBarSystem : ECSSystem
    {
        public static float PROGRESS_LOADING_TIMEOUT = 120f;
        private HashSet<AssetBundleInfo> loadingBundles = new HashSet<AssetBundleInfo>();
        private List<AssetBundleInfo> canceledBundles = new List<AssetBundleInfo>();
        private float lastProgress;
        private float lastUpdateTime;

        private void CancelUnactualBundles(LoadBundlesTaskComponent loadBundlesTask, ICollection<SingleNode<ResourceLoadStatComponent>> loadStats)
        {
            HashSet<AssetBundleInfo> trackedBundles = loadBundlesTask.TrackedBundles;
            this.loadingBundles.Clear();
            this.canceledBundles.Clear();
            foreach (SingleNode<ResourceLoadStatComponent> node in loadStats)
            {
                foreach (AssetBundleInfo info in node.component.BundleToProgress.Keys)
                {
                    this.loadingBundles.Add(info);
                }
            }
            foreach (AssetBundleInfo info2 in trackedBundles)
            {
                if (!this.loadingBundles.Contains(info2))
                {
                    this.canceledBundles.Add(info2);
                }
            }
            foreach (AssetBundleInfo info3 in this.canceledBundles)
            {
                loadBundlesTask.CancelBundle(info3);
            }
        }

        [OnEventFire]
        public void DisableBackButton(NodeAddedEvent e, SingleNode<ResourcesLoadProgressBarComponent> screen, [JoinAll, Combine] SingleNode<BackButtonComponent> backButton)
        {
            backButton.component.Disabled = true;
        }

        [OnEventFire]
        public void PrepareLoadTask(ProgressBarCalucationEvent e, ProgressBarNode node, [JoinAll] ICollection<AssetBundleNode> loadingBundles)
        {
            HashSet<AssetBundleInfo> trackedBundles = new HashSet<AssetBundleInfo>();
            foreach (AssetBundleNode node2 in loadingBundles)
            {
                foreach (AssetBundleInfo info in node2.assetBundlesLoadData.BundlesToLoad)
                {
                    if (!AssetBundlesStorage.IsStored(info))
                    {
                        trackedBundles.Add(info);
                    }
                }
                foreach (AssetBundleInfo info2 in node2.assetBundlesLoadData.LoadingBundles)
                {
                    if (!AssetBundlesStorage.IsStored(info2))
                    {
                        trackedBundles.Add(info2);
                    }
                }
            }
            LoadBundlesTaskComponent component = new LoadBundlesTaskComponent(trackedBundles);
            node.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void PrepareProgressDelayed(NodeAddedEvent e, ProgressBarNode node, SingleNode<AssetBundleDatabaseComponent> assetDb)
        {
            base.NewEvent<ProgressBarCalucationEvent>().Attach(node.Entity).ScheduleDelayed(node.resourcesLoadProgressBar.TimeBeforeProgressCalculation);
        }

        private bool ShowLogIfTimeout(float progress, LoadBundlesTaskComponent loadBundlesTask, ICollection<SingleNode<AssetBundleLoadingComponent>> assetBundleLoading)
        {
            bool flag = true;
            if (progress != this.lastProgress)
            {
                this.lastUpdateTime = Time.realtimeSinceStartup;
            }
            else if ((Time.realtimeSinceStartup - this.lastUpdateTime) > PROGRESS_LOADING_TIMEOUT)
            {
                this.lastUpdateTime = Time.realtimeSinceStartup;
                Console.WriteLine("Timeout on tracked bundles loading");
                Console.WriteLine("Tracked bundles:");
                foreach (AssetBundleInfo info in loadBundlesTask.TrackedBundles)
                {
                    Console.WriteLine(info.Filename);
                }
                Console.WriteLine("ResourceLoadStats:");
                foreach (SingleNode<AssetBundleLoadingComponent> node in assetBundleLoading)
                {
                    Console.WriteLine("LoadingBundle: {0}, state: {1}", node.component.AssetBundleDiskCacheRequest.AssetBundleInfo, node.component.AssetBundleDiskCacheRequest.State);
                }
                flag = false;
            }
            this.lastProgress = progress;
            return flag;
        }

        private void UpdateLoadBundlesTask(LoadBundlesTaskComponent loadBundlesTask, ICollection<SingleNode<ResourceLoadStatComponent>> loadStats)
        {
            HashSet<AssetBundleInfo> trackedBundles = loadBundlesTask.TrackedBundles;
            foreach (AssetBundleInfo info in loadBundlesTask.TrackedBundles.ToArray<AssetBundleInfo>())
            {
                if (AssetBundlesStorage.IsStored(info))
                {
                    loadBundlesTask.MarkBundleAsLoaded(info);
                }
            }
            this.CancelUnactualBundles(loadBundlesTask, loadStats);
            foreach (SingleNode<ResourceLoadStatComponent> node in loadStats)
            {
                foreach (KeyValuePair<AssetBundleInfo, float> pair in node.component.BundleToProgress)
                {
                    AssetBundleInfo key = pair.Key;
                    if (trackedBundles.Contains(key))
                    {
                        loadBundlesTask.SetBundleProgressLoading(key, pair.Value);
                    }
                }
            }
        }

        [OnEventComplete]
        public void UpdateProgress(UpdateEvent e, CalculatedProgressBarNode node, [JoinAll] ICollection<SingleNode<ResourceLoadStatComponent>> loadStats, [JoinAll] ICollection<SingleNode<AssetBundleLoadingComponent>> assetBundlesLoading)
        {
            LoadBundlesTaskComponent loadBundlesTask = node.loadBundlesTask;
            this.UpdateLoadBundlesTask(loadBundlesTask, loadStats);
            node.loadBundlesTaskProvider.UpdateData(loadBundlesTask);
            if (loadBundlesTask.AllBundlesLoaded())
            {
                base.Log.Info("ProgressComplete");
                node.Entity.RemoveComponent<LoadBundlesTaskComponent>();
                node.Entity.AddComponentIfAbsent<LoadProgressTaskCompleteComponent>();
            }
            else if (!node.Entity.HasComponent<LoadProgressTaskCompleteComponent>() && this.ShowLogIfTimeout(node.resourcesLoadProgressBar.ProgressBar.ProgressValue, loadBundlesTask, assetBundlesLoading))
            {
                base.Log.Info("LoadingComplete by timeout");
                node.Entity.AddComponent<LoadProgressTaskCompleteComponent>();
            }
        }

        public class AssetBundleNode : Node
        {
            public AssetBundlesLoadDataComponent assetBundlesLoadData;
        }

        public class CalculatedProgressBarNode : Node
        {
            public ResourcesLoadProgressBarComponent resourcesLoadProgressBar;
            public LoadBundlesTaskComponent loadBundlesTask;
            public LoadBundlesTaskProviderComponent loadBundlesTaskProvider;
        }

        public class ProgressBarCalucationEvent : Event
        {
        }

        public class ProgressBarNode : Node
        {
            public ResourcesLoadProgressBarComponent resourcesLoadProgressBar;
            public LoadBundlesTaskProviderComponent loadBundlesTaskProvider;
        }
    }
}

