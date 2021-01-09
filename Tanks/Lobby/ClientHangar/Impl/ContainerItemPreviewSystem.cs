namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientGraphics.Impl;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;

    public class ContainerItemPreviewSystem : ItemPreviewBaseSystem
    {
        [OnEventFire]
        public void HideGraffiti(NodeAddedEvent e, ContainerContentScreenNode screen, [JoinAll] ItemPreviewBaseSystem.GraffitiPreviewNode graffiti)
        {
            graffiti.Entity.RemoveComponent<HangarItemPreviewComponent>();
            screen.graffitiPreview.ResetPreview();
        }

        [OnEventComplete]
        public void ItemSelected(ListItemSelectedEvent e, ContainerContentItemNotPreviewNode containerContantItem)
        {
            this.PreviewContainerContentItem(containerContantItem.Entity);
        }

        [OnEventComplete]
        public void ItemSelected(JoinParentItemEvent e, SingleNode<SkinItemComponent> skin, [JoinByParentGroup] HullItemNotPreviewNode hull)
        {
            base.PreviewItem(hull.Entity);
        }

        [OnEventComplete]
        public void ItemSelected(JoinParentItemEvent e, SingleNode<SkinItemComponent> skin, [JoinByParentGroup] WeaponItemNotPreviewNode weapon)
        {
            base.PreviewItem(weapon.Entity);
        }

        [OnEventFire]
        public void PreviewContainer(NodeAddedEvent e, HangarNode hangar, HangarCameraNode hangarCamera, SingleNode<MandatoryAssetsFirstLoadingCompletedComponent> completedLoading, [JoinAll] ICollection<ContainerMarketItemNode> containers, [JoinAll] ICollection<ContainerItemPreviewNode> previewContainers)
        {
            if ((previewContainers.Count == 0) && (containers.Count > 0))
            {
                base.PreviewItem(containers.First<ContainerMarketItemNode>().Entity);
            }
        }

        public void PreviewContainerContentItem(Entity item)
        {
            <PreviewContainerContentItem>c__AnonStorey0 storey = new <PreviewContainerContentItem>c__AnonStorey0 {
                item = item
            };
            base.Select<ContainerContentItemPreviewNode>(storey.item, typeof(ContainerGroupComponent)).ForEach<ContainerContentItemPreviewNode>(new Action<ContainerContentItemPreviewNode>(storey.<>m__0));
            if (!storey.item.HasComponent<ContainerContentItemPreviewComponent>())
            {
                storey.item.AddComponent<ContainerContentItemPreviewComponent>();
            }
        }

        [OnEventFire]
        public void PreviewContent(NodeAddedEvent e, SimpleContainerContentItemPreviewNode containerContentItem)
        {
            Entity item = Flow.Current.EntityRegistry.GetEntity(containerContentItem.simpleContainerContentItem.MarketItemId);
            base.PreviewItem(item);
            if (item.HasComponent<SkinItemComponent>())
            {
                base.ScheduleEvent<JoinParentItemEvent>(item);
            }
        }

        [OnEventFire]
        public void ResetGraffiti(PreviewMountedItemsEvent e, SingleNode<GraffitiItemComponent> graffiti, [JoinAll] ContainerContentScreenNode screen)
        {
            screen.graffitiPreview.ResetPreview();
        }

        [OnEventFire]
        public void RevertToMounted(NodeRemoveEvent e, ContainerContentScreenNode screen, [JoinAll] ContainerContentItemPreviewNode containerContentItem)
        {
            containerContentItem.Entity.RemoveComponent<ContainerContentItemPreviewComponent>();
        }

        [OnEventFire]
        public void RevertToMounted(PreviewMountedItemsEvent e, SingleNode<GarageItemComponent> item, [JoinAll] ContainerContentScreenNode screen, [JoinAll, Combine] ItemPreviewBaseSystem.MountedUserItemNode mountedItem)
        {
            if (!mountedItem.Entity.HasComponent<GraffitiItemComponent>())
            {
                base.PreviewItem(mountedItem.Entity);
            }
        }

        [OnEventFire]
        public void UnpreviewContent(NodeRemoveEvent e, SimpleContainerContentItemPreviewNode containerContentItem, [JoinAll] ContainerContentScreenNode screen)
        {
            Entity entity = Flow.Current.EntityRegistry.GetEntity(containerContentItem.simpleContainerContentItem.MarketItemId);
            base.ScheduleEvent<PreviewMountedItemsEvent>(entity);
        }

        [CompilerGenerated]
        private sealed class <PreviewContainerContentItem>c__AnonStorey0
        {
            internal Entity item;

            internal void <>m__0(ContainerItemPreviewSystem.ContainerContentItemPreviewNode p)
            {
                if (!ReferenceEquals(p.Entity, this.item))
                {
                    p.Entity.RemoveComponent<ContainerContentItemPreviewComponent>();
                }
            }
        }

        [Not(typeof(ContainerContentItemPreviewComponent))]
        public class ContainerContentItemNotPreviewNode : Node
        {
            public ContainerContentItemComponent containerContentItem;
        }

        public class ContainerContentItemPreviewNode : Node
        {
            public ContainerContentItemComponent containerContentItem;
            public ContainerContentItemPreviewComponent containerContentItemPreview;
        }

        public class ContainerContentScreenNode : Node
        {
            public ContainerContentScreenComponent containerContentScreen;
            public GraffitiPreviewComponent graffitiPreview;
            public ActiveScreenComponent activeScreen;
        }

        public class ContainerItemPreviewNode : Node
        {
            public ContainerMarkerComponent containerMarker;
            public HangarItemPreviewComponent hangarItemPreview;
        }

        public class ContainerMarketItemNode : Node
        {
            public ContainerMarkerComponent containerMarker;
            public MarketItemComponent marketItem;
            public ResourceDataComponent resourceData;
        }

        public class HangarCameraNode : Node
        {
            public CameraComponent camera;
            public HangarComponent hangar;
        }

        public class HangarNode : Node
        {
            public HangarTankPositionComponent hangarTankPosition;
            public HangarContainerPositionComponent hangarContainerPosition;
        }

        [Not(typeof(HangarItemPreviewComponent))]
        public class HullItemNotPreviewNode : Node
        {
            public TankItemComponent tankItem;
            public MarketItemComponent marketItem;
        }

        public class JoinParentItemEvent : Event
        {
        }

        public class PreviewMountedItemsEvent : Event
        {
        }

        public class SimpleContainerContentItemPreviewNode : ContainerItemPreviewSystem.ContainerContentItemPreviewNode
        {
            public SimpleContainerContentItemComponent simpleContainerContentItem;
        }

        [Not(typeof(HangarItemPreviewComponent))]
        public class WeaponItemNotPreviewNode : Node
        {
            public WeaponItemComponent weaponItem;
            public MarketItemComponent marketItem;
        }
    }
}

