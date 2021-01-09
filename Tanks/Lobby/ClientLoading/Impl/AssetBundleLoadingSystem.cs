namespace Tanks.Lobby.ClientLoading.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientLoading.API;

    public class AssetBundleLoadingSystem : ECSSystem
    {
        [OnEventFire]
        public void DecreaseAssetBundleLoadingChannelsCount(NodeAddedEvent e, BattleLoadCompletedNode battleLoadCompleted, SingleNode<MapInstanceComponent> map, [JoinAll] SingleNode<AssetBundleLoadingChannelsCountComponent> assetBundleLoadingChannelsCount)
        {
            assetBundleLoadingChannelsCount.component.ChannelsCount = 1;
        }

        [OnEventFire]
        public void IncreaseAssetBundleLoadingChannelsCount(NodeRemoveEvent e, SingleNode<MapInstanceComponent> map, [JoinAll] SingleNode<AssetBundleLoadingChannelsCountComponent> assetBundleLoadingChannelsCount)
        {
            assetBundleLoadingChannelsCount.component.ChannelsCount = assetBundleLoadingChannelsCount.component.DefaultChannelsCount;
        }

        public class BattleLoadCompletedNode : Node
        {
            public LoadProgressTaskCompleteComponent loadProgressTaskComplete;
            public BattleLoadScreenComponent battleLoadScreen;
        }
    }
}

