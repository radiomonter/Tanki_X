namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientHangar.Impl.Builder;

    public class HangarTankLoadSystem : HangarTankBaseSystem
    {
        [OnEventFire]
        public void CancelItemAssetRequest(NodeRemoveEvent e, HangarTankBaseSystem.TankItemPreviewNode parent, [JoinByParentGroup] LoadingSkinPreviewItemNode item)
        {
            item.Entity.RemoveComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void CancelItemAssetRequest(NodeRemoveEvent e, HangarTankBaseSystem.WeaponItemPreviewNode weapon, [JoinByParentGroup] LoadingSkinPreviewItemNode skin)
        {
            skin.Entity.RemoveComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void HideLoadIndicator(NodeAddedEvent e, LoadedGraffitiHangarPreviewItemNode graffiti, [JoinAll] GraffitiLoadGearNode loadGear)
        {
            base.ScheduleEvent<HideLoadGearEvent>(loadGear);
        }

        [OnEventFire]
        public void HideLoadIndicator(NodeRemoveEvent e, SingleNode<HangarInstanceComponent> hangar, [Combine, JoinAll] SingleNode<LoadGearComponent> loadGear)
        {
            base.ScheduleEvent<HideLoadGearEvent>(loadGear);
        }

        [OnEventFire]
        public void HideLoadIndicator(HangarGraffitiBuildedEvent e, Node node, [JoinAll] GraffitiLoadGearNode loadGear)
        {
            base.ScheduleEvent<HideLoadGearEvent>(loadGear);
        }

        [OnEventFire]
        public void HideLoadIndicator(NodeAddedEvent e, HangarTankBaseSystem.WeaponItemPreviewNode weapon, [Context, JoinByParentGroup] LoadedSkinPreviewItemNode weaponSkin, HangarTankBaseSystem.TankItemPreviewNode tank, [Context, JoinByParentGroup] LoadedSkinPreviewItemNode tankSkin, LoadedTankColoringPreviewItemNode tankColoring, LoadedWeaponColoringPreviewItemNode weaponColoring, [JoinAll] TankLoadGearNode loadGear)
        {
            base.ScheduleEvent<HideLoadGearEvent>(loadGear);
        }

        [OnEventFire]
        public void HideLoadIndicator(NodeAddedEvent e, LoadedContainerPreviewItemNode container, LoadedTankColoringPreviewItemNode tankColoring, LoadedWeaponColoringPreviewItemNode weaponColoring, HangarTankBaseSystem.WeaponItemPreviewNode weapon, [JoinByParentGroup, Context] LoadedSkinPreviewItemNode weaponSkin, HangarTankBaseSystem.TankItemPreviewNode tank, [Context, JoinByParentGroup] LoadedSkinPreviewItemNode tankSkin, [JoinAll] TankLoadGearNode loadGear)
        {
            base.ScheduleEvent<HideLoadGearEvent>(loadGear);
        }

        [OnEventFire]
        public void RequestItemAsset(NodeAddedEvent e, NotLoadedContainerPreviewNode item, SingleNode<MandatoryAssetsFirstLoadingCompletedComponent> mandatoryAssetsLoadingComplete)
        {
            item.Entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void RequestItemAsset(NodeAddedEvent e, NotLoadedGraffityPreviewNode item, SingleNode<MandatoryAssetsFirstLoadingCompletedComponent> mandatoryAssetsLoadingComplete)
        {
            item.Entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void RequestItemAsset(NodeAddedEvent e, NotLoadedTankColoringPreviewNode item, SingleNode<MandatoryAssetsFirstLoadingCompletedComponent> mandatoryAssetsLoadingComplete)
        {
            item.Entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void RequestItemAsset(NodeAddedEvent e, NotLoadedWeaponColoringPreviewNode item, SingleNode<MandatoryAssetsFirstLoadingCompletedComponent> mandatoryAssetsLoadingComplete)
        {
            item.Entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void RequestItemAsset(NodeAddedEvent e, HangarTankBaseSystem.TankItemPreviewNode tank, [Context, JoinByParentGroup] NotLoadedSkinPreviewItemNode skinItem, SingleNode<MandatoryAssetsFirstLoadingCompletedComponent> mandatoryAssetsLoadingComplete)
        {
            skinItem.Entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void RequestItemAsset(NodeAddedEvent e, HangarTankBaseSystem.WeaponItemPreviewNode weapon, [Context, JoinByParentGroup] NotLoadedSkinPreviewItemNode skinItem, SingleNode<MandatoryAssetsFirstLoadingCompletedComponent> mandatoryAssetsLoadingComplete)
        {
            skinItem.Entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void ShowLoadIndicator(NodeAddedEvent e, SingleNode<HangarInstanceComponent> hangar, [Combine] LoadingContainerPreviewNode container, [JoinAll] TankLoadGearNode loadGear)
        {
            base.ScheduleEvent(new ShowLoadGearEvent(true), loadGear);
        }

        [OnEventFire]
        public void ShowLoadIndicator(NodeAddedEvent e, SingleNode<HangarInstanceComponent> hangar, [Combine] LoadingGraffitiHangarPreviewItemNode graffity, [JoinAll] GraffitiLoadGearNode loadGear)
        {
            base.ScheduleEvent(new ShowLoadGearEvent(true), loadGear);
        }

        [OnEventFire]
        public void ShowLoadIndicator(NodeAddedEvent e, SingleNode<HangarInstanceComponent> hangar, LoadingTankColoringPreviewNode tankColoring, [JoinAll] TankLoadGearNode loadGear)
        {
            base.ScheduleEvent(new ShowLoadGearEvent(true), loadGear);
        }

        [OnEventFire]
        public void ShowLoadIndicator(NodeAddedEvent e, SingleNode<HangarInstanceComponent> hangar, LoadingWeaponColoringPreviewNode tankColoring, [JoinAll] TankLoadGearNode loadGear)
        {
            base.ScheduleEvent(new ShowLoadGearEvent(true), loadGear);
        }

        [OnEventFire]
        public void ShowLoadIndicator(NodeAddedEvent e, SingleNode<HangarInstanceComponent> hangar, HangarTankBaseSystem.TankItemPreviewNode tank, [Context, JoinByParentGroup] LoadingSkinPreviewItemNode tankSkin, [JoinAll] TankLoadGearNode loadGear)
        {
            base.ScheduleEvent(new ShowLoadGearEvent(true), loadGear);
        }

        [OnEventFire]
        public void ShowLoadIndicator(NodeAddedEvent e, SingleNode<HangarInstanceComponent> hangar, HangarTankBaseSystem.WeaponItemPreviewNode tank, [Context, JoinByParentGroup] LoadingSkinPreviewItemNode tankSkin, [JoinAll] TankLoadGearNode loadGear)
        {
            base.ScheduleEvent(new ShowLoadGearEvent(true), loadGear);
        }

        [OnEventFire]
        public void UpdateGearProgress(UpdateEvent evt, ActiveGraffitiLoadGearNode gear, [JoinAll] ICollection<LoadingGraffitiHangarPreviewItemStatsNode> items)
        {
            int num = 0;
            int num2 = 0;
            foreach (LoadingHangarPreviewItemStatsNode node in items)
            {
                ResourceLoadStatComponent resourceLoadStat = node.resourceLoadStat;
                num += resourceLoadStat.BytesTotal;
                num2 += resourceLoadStat.BytesLoaded;
            }
            float num3 = (num <= 0) ? 1f : (((float) num2) / ((float) num));
            base.ScheduleEvent(new UpdateLoadGearProgressEvent(num3), gear);
        }

        [OnEventFire]
        public void UpdateGearProgress(UpdateEvent evt, ActiveTankLoadGearNode gear, [JoinAll] ICollection<LoadingNotGraffitiHangarPreviewItemStatsNode> items)
        {
            int num = 0;
            int num2 = 0;
            foreach (LoadingHangarPreviewItemStatsNode node in items)
            {
                ResourceLoadStatComponent resourceLoadStat = node.resourceLoadStat;
                num += resourceLoadStat.BytesTotal;
                num2 += resourceLoadStat.BytesLoaded;
            }
            float num3 = (num <= 0) ? 1f : (((float) num2) / ((float) num));
            base.ScheduleEvent(new UpdateLoadGearProgressEvent(num3), gear);
        }

        public class ActiveGraffitiLoadGearNode : HangarTankLoadSystem.GraffitiLoadGearNode
        {
            public ActiveGearComponent activeGear;
        }

        public class ActiveTankLoadGearNode : HangarTankLoadSystem.TankLoadGearNode
        {
            public ActiveGearComponent activeGear;
        }

        public class GraffitiLoadGearNode : Node
        {
            public GraffitiGarageItemsScreenComponent graffitiGarageItemsScreen;
            public LoadGearComponent loadGear;
        }

        public class LoadedContainerPreviewItemNode : HangarTankLoadSystem.LoadedHangarPreviewItemNode
        {
            public ContainerMarkerComponent containerMarker;
        }

        public class LoadedGraffitiHangarPreviewItemNode : HangarTankLoadSystem.LoadedHangarPreviewItemNode
        {
            public GraffitiItemComponent graffitiItem;
        }

        public class LoadedHangarPreviewItemNode : HangarTankBaseSystem.HangarPreviewItemNode
        {
            public ResourceDataComponent resourceData;
        }

        public class LoadedSkinPreviewItemNode : HangarTankLoadSystem.LoadedHangarPreviewItemNode
        {
            public SkinItemComponent skinItem;
            public ParentGroupComponent parentGroup;
        }

        public class LoadedTankColoringPreviewItemNode : HangarTankLoadSystem.LoadedHangarPreviewItemNode
        {
            public TankPaintItemComponent tankPaintItem;
        }

        public class LoadedWeaponColoringPreviewItemNode : HangarTankLoadSystem.LoadedHangarPreviewItemNode
        {
            public WeaponPaintItemComponent weaponPaintItem;
        }

        public class LoadingContainerPreviewNode : HangarTankLoadSystem.LoadingHangarPreviewItemNode
        {
            public ContainerMarkerComponent containerMarker;
        }

        public class LoadingGraffitiHangarPreviewItemNode : HangarTankLoadSystem.LoadingHangarPreviewItemNode
        {
            public GraffitiItemComponent graffitiItem;
        }

        public class LoadingGraffitiHangarPreviewItemStatsNode : HangarTankLoadSystem.LoadingHangarPreviewItemStatsNode
        {
            public GraffitiItemComponent graffitiItem;
        }

        [Not(typeof(ResourceDataComponent))]
        public class LoadingHangarPreviewItemNode : HangarTankBaseSystem.HangarPreviewItemNode
        {
            public AssetRequestComponent assetRequest;
            public AssetReferenceComponent assetReference;
        }

        [Not(typeof(ResourceDataComponent))]
        public class LoadingHangarPreviewItemStatsNode : HangarTankLoadSystem.LoadingHangarPreviewItemNode
        {
            public ResourceLoadStatComponent resourceLoadStat;
        }

        [Not(typeof(GraffitiItemComponent))]
        public class LoadingNotGraffitiHangarPreviewItemStatsNode : HangarTankLoadSystem.LoadingHangarPreviewItemStatsNode
        {
        }

        public class LoadingSkinPreviewItemNode : HangarTankLoadSystem.LoadingHangarPreviewItemNode
        {
            public SkinItemComponent skinItem;
        }

        public class LoadingTankColoringPreviewNode : HangarTankLoadSystem.LoadingHangarPreviewItemNode
        {
            public TankPaintItemComponent tankPaintItem;
        }

        public class LoadingWeaponColoringPreviewNode : HangarTankLoadSystem.LoadingHangarPreviewItemNode
        {
            public WeaponPaintItemComponent weaponPaintItem;
        }

        [Not(typeof(ResourceDataComponent)), Not(typeof(AssetRequestComponent))]
        public class NotLoadedContainerPreviewNode : HangarTankBaseSystem.HangarPreviewItemNode
        {
            public ContainerMarkerComponent containerMarker;
        }

        [Not(typeof(ResourceDataComponent)), Not(typeof(AssetRequestComponent))]
        public class NotLoadedGraffityPreviewNode : HangarTankBaseSystem.HangarPreviewItemNode
        {
            public GraffitiItemComponent graffitiItem;
        }

        [Not(typeof(ResourceDataComponent)), Not(typeof(AssetRequestComponent))]
        public class NotLoadedSkinPreviewItemNode : HangarTankBaseSystem.HangarPreviewItemNode
        {
            public AssetReferenceComponent assetReference;
            public SkinItemComponent skinItem;
        }

        [Not(typeof(ResourceDataComponent)), Not(typeof(AssetRequestComponent))]
        public class NotLoadedTankColoringPreviewNode : HangarTankBaseSystem.HangarPreviewItemNode
        {
            public TankPaintItemComponent tankPaintItem;
        }

        [Not(typeof(ResourceDataComponent)), Not(typeof(AssetRequestComponent))]
        public class NotLoadedWeaponColoringPreviewNode : HangarTankBaseSystem.HangarPreviewItemNode
        {
            public WeaponPaintItemComponent weaponPaintItem;
        }

        public class TankLoadGearNode : Node
        {
            public ScreenForegroundComponent screenForeground;
            public LoadGearComponent loadGear;
        }
    }
}

