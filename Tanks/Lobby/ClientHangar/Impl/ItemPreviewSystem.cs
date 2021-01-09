namespace Tanks.Lobby.ClientHangar.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientNavigation.API;

    public class ItemPreviewSystem : ItemPreviewBaseSystem
    {
        [CompilerGenerated]
        private static Action<ItemPreviewBaseSystem.GraffitiPreviewNode> <>f__am$cache0;

        [OnEventFire]
        public void AddMarketDefaultSkin(NodeAddedEvent e, DefaultSkinItemNode defaultSkin)
        {
            defaultSkin.Entity.AddComponent<HangarItemPreviewComponent>();
        }

        [OnEventFire]
        public void AddPreviewGroup(NodeAddedEvent e, SingleNode<GarageItemComponent> garageItem)
        {
            DirectoryInfo parent = Directory.GetParent(((EntityInternal) garageItem.Entity).TemplateAccessor.Get().ConfigPath);
            garageItem.Entity.AddComponent(new PreviewGroupComponent((long) ConfigurationEntityIdCalculator.Calculate(parent.ToString())));
        }

        public void ApplyPreview(Entity item)
        {
            <ApplyPreview>c__AnonStorey0 storey = new <ApplyPreview>c__AnonStorey0 {
                item = item
            };
            base.Select<ItemPreviewBaseSystem.PreviewNode>(storey.item, typeof(PreviewGroupComponent)).ForEach<ItemPreviewBaseSystem.PreviewNode>(new Action<ItemPreviewBaseSystem.PreviewNode>(storey.<>m__0));
            if (!storey.item.HasComponent<HangarItemPreviewComponent>())
            {
                storey.item.AddComponent<HangarItemPreviewComponent>();
            }
        }

        [OnEventFire]
        public void ApplyPreview(ItemPreviewBaseSystem.PrewievEvent e, ItemPreviewBaseSystem.NotGraffitiNode node)
        {
            this.ApplyPreview(node.Entity);
        }

        [OnEventComplete]
        public void ApplyPreview(ItemPreviewBaseSystem.PrewievEvent e, GraffitiNotPreviewNode graffiti, [JoinAll] ICollection<ItemPreviewBaseSystem.GraffitiPreviewNode> otherGraffiti)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = o => o.Entity.RemoveComponent<HangarItemPreviewComponent>();
            }
            otherGraffiti.ForEach<ItemPreviewBaseSystem.GraffitiPreviewNode>(<>f__am$cache0);
            graffiti.Entity.AddComponent<HangarItemPreviewComponent>();
        }

        [OnEventFire]
        public void CleanCurrentPreview(ResetPreviewEvent e, Node any, [JoinAll] ICollection<SingleNode<ContainersUI>> containersUis, [JoinAll] ICollection<SingleNode<HangarItemPreviewComponent>> previewItems)
        {
            <CleanCurrentPreview>c__AnonStorey1 storey = new <CleanCurrentPreview>c__AnonStorey1 {
                e = e,
                except = new List<Entity>()
            };
            foreach (SingleNode<ContainersUI> node in containersUis)
            {
                if (!ReferenceEquals(node.Entity, any))
                {
                    node.component.GetContainers().ForEach(new Action<ContainerBoxItem>(storey.<>m__0));
                }
            }
            previewItems.ForEach<SingleNode<HangarItemPreviewComponent>>(new Action<SingleNode<HangarItemPreviewComponent>>(storey.<>m__1));
        }

        [OnEventComplete]
        public void DefaultItemsPreview(ResetPreviewEvent e, Node any, [JoinAll] PresetNode selectedPreset, [JoinByUser] ICollection<ItemPreviewBaseSystem.MountedUserItemNode> mountedItems, [JoinAll] ICollection<DefaultSkinItemNode> defaultSkins)
        {
            <DefaultItemsPreview>c__AnonStorey2 storey = new <DefaultItemsPreview>c__AnonStorey2 {
                e = e,
                $this = this
            };
            defaultSkins.ForEach<DefaultSkinItemNode>(new Action<DefaultSkinItemNode>(storey.<>m__0));
            mountedItems.ForEach<ItemPreviewBaseSystem.MountedUserItemNode>(new Action<ItemPreviewBaseSystem.MountedUserItemNode>(storey.<>m__1));
        }

        [OnEventComplete]
        public void ItemSelected(ListItemSelectedEvent e, SingleNode<ContainerMarkerComponent> container)
        {
            base.PreviewItem(container.Entity);
        }

        [OnEventComplete]
        public void ItemSelected(ListItemSelectedEvent e, SingleNode<GarageItemComponent> item, [JoinAll] GarageItemsPreviewScreenNode screen)
        {
            base.PreviewItem(item.Entity);
        }

        [OnEventComplete]
        public void ItemSelected(ListItemSelectedEvent e, SingleNode<SkinItemComponent> skin, [JoinByParentGroup] ICollection<ItemPreviewBaseSystem.HulLPreviewNode> hulls)
        {
            using (IEnumerator<ItemPreviewBaseSystem.HulLPreviewNode> enumerator = hulls.GetEnumerator())
            {
                while (true)
                {
                    if (!enumerator.MoveNext())
                    {
                        break;
                    }
                    ItemPreviewBaseSystem.HulLPreviewNode current = enumerator.Current;
                    if (current.Entity.HasComponent<UserGroupComponent>())
                    {
                        base.PreviewItem(current.Entity);
                        return;
                    }
                }
            }
            ItemPreviewBaseSystem.HulLPreviewNode node2 = hulls.FirstOrDefault<ItemPreviewBaseSystem.HulLPreviewNode>();
            if (node2 != null)
            {
                base.PreviewItem(node2.Entity);
            }
        }

        [OnEventComplete]
        public void ItemSelected(ListItemSelectedEvent e, SingleNode<SkinItemComponent> skin, [JoinByParentGroup] ICollection<ItemPreviewBaseSystem.WeaponNotPreviewNode> weapons)
        {
            ItemPreviewBaseSystem.WeaponNotPreviewNode node = weapons.FirstOrDefault<ItemPreviewBaseSystem.WeaponNotPreviewNode>();
            if (node != null)
            {
                base.PreviewItem(node.Entity);
            }
        }

        [OnEventFire]
        public void OnModulesEnter(NodeAddedEvent e, ActiveModulesScreenNode modulesScreen, [JoinByScreen] SingleNode<GarageScreenContextComponent> contextNode)
        {
            base.PreviewItem(contextNode.component.ContextItem);
        }

        [OnEventFire]
        public void PreviewMounted(NodeAddedEvent e, [Combine] NotGraffityMountedNode mountedItem, [Context, JoinByUser] PresetNode preset)
        {
            base.PreviewItem(mountedItem.Entity);
        }

        [OnEventFire]
        public void RemovePreview(ItemPreviewBaseSystem.PrewievEvent e, ItemPreviewBaseSystem.NotGraffitiNode notGraffiti, [JoinAll, Combine] GraffitiWithPreviewNode previewedGraffiti)
        {
            previewedGraffiti.Entity.RemoveComponent<HangarItemPreviewComponent>();
        }

        [OnEventComplete]
        public void ResetPreview(NodeAddedEvent e, ContainerContentScreenHidingNode screen)
        {
            base.ScheduleEvent<ResetPreviewEvent>(screen);
        }

        [OnEventFire]
        public void RevertToMounted(NodeAddedEvent e, SingleNode<ScreenComponent> screen, [JoinAll] PresetNode preset, [JoinByUser, Combine] NotGraffityMountedNode mountedItem)
        {
            if (screen.component.gameObject.GetComponent<ItemsListScreenComponent>() == null)
            {
                base.PreviewItem(mountedItem.Entity);
            }
        }

        [OnEventFire]
        public void RevertToMountedForSkins(NodeRemoveEvent e, SingleNode<GarageItemsScreenComponent> itemsScreen, [JoinAll] PresetNode preset, [JoinByUser, Combine] MountedUserSkinNode mountedItem)
        {
            base.PreviewItem(mountedItem.Entity);
        }

        [OnEventComplete]
        public void ShellSelected(ListItemSelectedEvent e, SingleNode<ShellItemComponent> shell, [JoinByParentGroup] ICollection<ItemPreviewBaseSystem.WeaponNotPreviewNode> weapons, [JoinAll] GarageItemsScreenNode screen)
        {
            ItemPreviewBaseSystem.WeaponNotPreviewNode node = weapons.FirstOrDefault<ItemPreviewBaseSystem.WeaponNotPreviewNode>();
            if (node != null)
            {
                base.PreviewItem(node.Entity);
            }
        }

        [CompilerGenerated]
        private sealed class <ApplyPreview>c__AnonStorey0
        {
            internal Entity item;

            internal void <>m__0(ItemPreviewBaseSystem.PreviewNode p)
            {
                if (!ReferenceEquals(p.Entity, this.item))
                {
                    p.Entity.RemoveComponent<HangarItemPreviewComponent>();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <CleanCurrentPreview>c__AnonStorey1
        {
            internal List<Entity> except;
            internal ResetPreviewEvent e;

            internal void <>m__0(ContainerBoxItem cb)
            {
                this.except.Add(cb.MarketItem);
                if (cb.UserItem != null)
                {
                    this.except.Add(cb.UserItem);
                }
            }

            internal void <>m__1(SingleNode<HangarItemPreviewComponent> item)
            {
                if (!this.except.Contains(item.Entity) && (!item.Entity.HasComponent<PreviewGroupComponent>() || (item.Entity.GetComponent<PreviewGroupComponent>().Key != this.e.ExceptPreviewGroup)))
                {
                    item.Entity.RemoveComponent<HangarItemPreviewComponent>();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <DefaultItemsPreview>c__AnonStorey2
        {
            internal ResetPreviewEvent e;
            internal ItemPreviewSystem $this;

            internal void <>m__0(ItemPreviewSystem.DefaultSkinItemNode item)
            {
                if (!item.Entity.HasComponent<HangarItemPreviewComponent>() && (!item.Entity.HasComponent<PreviewGroupComponent>() || (item.Entity.GetComponent<PreviewGroupComponent>().Key != this.e.ExceptPreviewGroup)))
                {
                    item.Entity.AddComponent<HangarItemPreviewComponent>();
                }
            }

            internal void <>m__1(ItemPreviewBaseSystem.MountedUserItemNode item)
            {
                this.$this.ApplyPreview(item.Entity);
            }
        }

        public class ActiveModulesScreenNode : ItemPreviewSystem.ActiveScreenNode
        {
            public GarageModulesScreenComponent garageModulesScreen;
        }

        public class ActiveScreenNode : Node
        {
            public ScreenComponent screen;
            public ActiveScreenComponent activeScreen;
        }

        public class ContainerContentScreenHidingNode : Node
        {
            public ContainerContentScreenComponent containerContentScreen;
            public ScreenHidingComponent screenHiding;
        }

        public class DefaultSkinItemNode : Node
        {
            public MarketItemComponent marketItem;
            public ParentGroupComponent parentGroup;
            public DefaultSkinItemComponent defaultSkinItem;
        }

        public class GarageItemsPreviewScreenNode : ItemPreviewSystem.ActiveScreenNode
        {
            public GarageItemsPreviewScreenComponent garageItemsPreviewScreen;
        }

        public class GarageItemsScreenNode : ItemPreviewSystem.ActiveScreenNode
        {
            public GarageItemsScreenComponent garageItemsScreen;
        }

        [Not(typeof(HangarItemPreviewComponent))]
        public class GraffitiNotPreviewNode : Node
        {
            public GraffitiItemComponent graffitiItem;
        }

        public class GraffitiWithPreviewNode : Node
        {
            public GraffitiItemComponent graffitiItem;
            public HangarItemPreviewComponent hangarItemPreview;
        }

        [Not(typeof(HangarItemPreviewComponent))]
        public class HullItemWithPropertiesNotPreviewNode : Node
        {
            public TankItemComponent tankItem;
        }

        public class MountedUserSkinNode : ItemPreviewBaseSystem.MountedUserItemNode
        {
            public SkinItemComponent skinItem;
        }

        public class NotGraffityMountedNode : ItemPreviewBaseSystem.NotGraffitiNode
        {
            public MountedItemComponent mountedItem;
        }

        public class PresetNode : Node
        {
            public PresetItemComponent presetItem;
            public SelectedPresetComponent selectedPreset;
        }

        [Not(typeof(HangarItemPreviewComponent))]
        public class WeaponItemWithPropertiesNotPreviewNode : Node
        {
            public WeaponItemComponent weaponItem;
        }
    }
}

