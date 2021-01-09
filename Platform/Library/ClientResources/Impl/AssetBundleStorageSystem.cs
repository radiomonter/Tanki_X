namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class AssetBundleStorageSystem : ECSSystem
    {
        private bool IsBundleInLoadingDependencies(AssetBundleInfo info, ICollection<AssetBundleLoadingNode> loadingBundleNodes)
        {
            <IsBundleInLoadingDependencies>c__AnonStorey0 storey = new <IsBundleInLoadingDependencies>c__AnonStorey0 {
                info = info
            };
            return loadingBundleNodes.Any<AssetBundleLoadingNode>(new Func<AssetBundleLoadingNode, bool>(storey.<>m__0));
        }

        [OnEventFire, Mandatory]
        public void RefreshDependencies(AssetBundlesLoadedEvent e, SingleNode<AssetBundlesLoadDataComponent> loadDataNode)
        {
            foreach (AssetBundleInfo info in loadDataNode.component.AllBundles)
            {
                AssetBundlesStorage.Refresh(info);
            }
        }

        [OnEventFire]
        public void Store(LoadCompleteEvent e, LoaderNode node)
        {
            if (!AssetBundlesStorage.IsStored(node.assetBundleLoading.Info))
            {
                AssetBundlesStorage.Store(node.assetBundleLoading.Info, node.assetBundleLoading.AssetBundleDiskCacheRequest.AssetBundle);
            }
        }

        [CompilerGenerated]
        private sealed class <IsBundleInLoadingDependencies>c__AnonStorey0
        {
            internal AssetBundleInfo info;

            internal bool <>m__0(AssetBundleStorageSystem.AssetBundleLoadingNode node) => 
                node.assetBundlesLoadData.AllBundles.Contains(this.info);
        }

        public class AssetBundleLoadingNode : Node
        {
            public AssetBundlesLoadDataComponent assetBundlesLoadData;
        }

        public class LoaderNode : Node
        {
            public AssetBundleLoadingComponent assetBundleLoading;
        }
    }
}

