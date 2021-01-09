namespace Platform.Library.ClientResources.API
{
    using Platform.Library.ClientResources.Impl;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class AssetBundleDiskCacheRequest
    {
        public AssetBundleDiskCacheRequest(Platform.Library.ClientResources.Impl.AssetBundleInfo assetBundleInfo, bool createAssetBundle = true)
        {
            this.State = AssetBundleDiskCacheState.INIT;
            this.AssetBundleInfo = assetBundleInfo;
            this.CreateAssetBundle = createAssetBundle;
            this.UseCrcCheck = true;
        }

        public bool UseCrcCheck { get; set; }

        public bool CreateAssetBundle { get; set; }

        public Platform.Library.ClientResources.Impl.AssetBundleInfo AssetBundleInfo { get; set; }

        public bool IsDone { get; set; }

        public UnityEngine.AssetBundle AssetBundle { get; set; }

        public string Error { get; set; }

        public float Progress { get; set; }

        public AssetBundleDiskCacheState State { get; set; }
    }
}

