namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class AssetLoadStatSystem : ECSSystem
    {
        private const float EPS = 0.001f;

        [OnEventFire]
        public void CalculateLoadSize(NodeAddedEvent e, LoadStatNode node, SingleNode<AssetBundleDatabaseComponent> db)
        {
            int num = 0;
            foreach (AssetBundleInfo info in node.assetBundlesLoadData.BundlesToLoad)
            {
                num += info.Size;
                node.resourceLoadStat.BundleToProgress[info] = 0f;
            }
            node.resourceLoadStat.BytesTotal = num;
        }

        [OnEventFire]
        public void UpdateLoadProgress(AssetBundlesLoadedEvent e, SingleNode<ResourceLoadStatComponent> node)
        {
            ResourceLoadStatComponent component = node.component;
            component.BytesLoaded = component.BytesTotal;
            component.Progress = 1f;
        }

        [OnEventFire]
        public void UpdateLoadProgress(UpdateEvent e, LoadingChannelNode loaderNode, [JoinBy(typeof(AssetGroupComponent))] LoadStatNode resource)
        {
            AssetBundleInfo key = loaderNode.assetBundleLoading.Info;
            ResourceLoadStatComponent resourceLoadStat = resource.resourceLoadStat;
            Dictionary<AssetBundleInfo, float> bundleToProgress = resourceLoadStat.BundleToProgress;
            if (!bundleToProgress.ContainsKey(key))
            {
                resourceLoadStat.Progress = 1f;
            }
            else
            {
                float progress = loaderNode.assetBundleLoading.AssetBundleDiskCacheRequest.Progress;
                bundleToProgress[key] = progress;
                if (Mathf.Abs((float) (progress - bundleToProgress[key])) >= 0.001f)
                {
                    float num3 = 0f;
                    Dictionary<AssetBundleInfo, float>.Enumerator enumerator = bundleToProgress.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        KeyValuePair<AssetBundleInfo, float> current = enumerator.Current;
                        num3 += current.Key.Size * current.Value;
                    }
                    resourceLoadStat.Progress = (resourceLoadStat.BytesTotal != 0) ? Mathf.Clamp01(num3 / ((float) resourceLoadStat.BytesTotal)) : 1f;
                    resourceLoadStat.BytesLoaded = Mathf.RoundToInt(resourceLoadStat.BytesTotal * resourceLoadStat.Progress);
                }
            }
        }

        public class LoadingChannelNode : Node
        {
            public AssetBundleLoadingComponent assetBundleLoading;
        }

        public class LoadStatNode : Node
        {
            public AssetBundlesLoadDataComponent assetBundlesLoadData;
            public ResourceLoadStatComponent resourceLoadStat;
        }
    }
}

