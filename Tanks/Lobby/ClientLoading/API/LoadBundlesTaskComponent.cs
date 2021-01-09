namespace Tanks.Lobby.ClientLoading.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientResources.Impl;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [SerialVersionUID(0x8d2e6e0b0b3761aL)]
    public class LoadBundlesTaskComponent : Component
    {
        private const int MB_MULTIPLIER = 0x100000;
        private Dictionary<AssetBundleInfo, float> bundleToProgress;
        private Dictionary<AssetBundleInfo, float> notCachedBundleToProgress;

        public LoadBundlesTaskComponent(HashSet<AssetBundleInfo> trackedBundles)
        {
            this.TrackedBundles = trackedBundles;
            this.bundleToProgress = new Dictionary<AssetBundleInfo, float>();
            this.notCachedBundleToProgress = new Dictionary<AssetBundleInfo, float>();
            float num = 0f;
            foreach (AssetBundleInfo info in trackedBundles)
            {
                this.bundleToProgress.Add(info, 0f);
                this.BytesToLoad += info.Size;
                if (!info.IsCached)
                {
                    this.notCachedBundleToProgress.Add(info, 0f);
                    num += info.Size;
                }
            }
            this.MBytesToLoadFromNetwork = (int) (num / 1048576f);
            this.LoadingStartTime = Time.realtimeSinceStartup;
        }

        public bool AllBundlesLoaded() => 
            this.TrackedBundles.Count == 0;

        public void CancelBundle(AssetBundleInfo bundleInfo)
        {
            this.TrackedBundles.Remove(bundleInfo);
        }

        public void MarkBundleAsLoaded(AssetBundleInfo bundleInfo)
        {
            this.bundleToProgress[bundleInfo] = bundleInfo.Size;
            if (this.notCachedBundleToProgress.ContainsKey(bundleInfo))
            {
                this.notCachedBundleToProgress[bundleInfo] = bundleInfo.Size;
            }
            this.TrackedBundles.Remove(bundleInfo);
        }

        public void SetBundleProgressLoading(AssetBundleInfo bundleInfo, float bundleProgress)
        {
            this.bundleToProgress[bundleInfo] = bundleProgress * bundleInfo.Size;
            if (this.notCachedBundleToProgress.ContainsKey(bundleInfo))
            {
                this.notCachedBundleToProgress[bundleInfo] = bundleProgress * bundleInfo.Size;
            }
        }

        public HashSet<AssetBundleInfo> TrackedBundles { get; set; }

        public int BytesToLoad { get; private set; }

        public int BytesLoaded =>
            (int) ((IEnumerable<float>) this.bundleToProgress.Values).Sum();

        public int MBytesToLoadFromNetwork { get; private set; }

        public int MBytesLoadedFromNetwork =>
            (int) (((IEnumerable<float>) this.notCachedBundleToProgress.Values).Sum() / 1048576f);

        public float LoadingStartTime { get; private set; }
    }
}

