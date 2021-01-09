namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientPayment.API;
    using UnityEngine;

    public class PresetSystem : ECSSystem
    {
        public static readonly PresetComparer comparer = new PresetComparer();
        private static readonly PresetNodeComparer nodeComparer = new PresetNodeComparer();
        [CompilerGenerated]
        private static Func<PresetListItemComponent, bool> <>f__am$cache0;
        [CompilerGenerated]
        private static Action <>f__am$cache1;
        [CompilerGenerated]
        private static Converter<PresetListItemComponent, Entity> <>f__am$cache2;

        public PresetSystem(SystemRegistry systemRegistry)
        {
            systemRegistry.RegisterSingleNode<CreateByRankConfigComponent>();
            systemRegistry.RegisterSingleNode<ItemsBuyCountLimitComponent>();
            systemRegistry.RegisterSingleNode<FirstBuySaleComponent>();
            systemRegistry.RegisterSingleNode<CreatedByRankItemComponent>();
        }

        private PresetListItemComponent AddListItem(PresetListNode presetList, BasePresetNode preset, SelfUserNode user)
        {
            PresetListItemComponent component = presetList.presetList.AddElement().GetComponent<PresetListItemComponent>();
            component.Preset = preset.Entity;
            component.IsUserItem = preset.Entity.HasComponent<UserItemComponent>();
            component.IsOwned = component.IsUserItem && (preset.Entity.GetComponent<UserGroupComponent>().Key == user.userGroup.Key);
            component.PresetName = !component.IsUserItem ? presetList.presetList.LockedByRankLocalizedText.Value : this.GetPresetName(preset.Entity);
            component.Locked = !component.IsUserItem;
            return component;
        }

        [OnEventFire]
        public void AddListItemOnNewPreset(NodeAddedEvent e, UserPresetNode preset, [JoinAll] PresetListNode presetList, [JoinAll] MarketPresetNode marketPreset, [JoinAll] SelfUserNode user)
        {
            <AddListItemOnNewPreset>c__AnonStorey3 storey = new <AddListItemOnNewPreset>c__AnonStorey3 {
                marketPreset = marketPreset
            };
            int num = GetListPresets(presetList).BinarySearch(preset.Entity, comparer);
            if (num < 0)
            {
                int index = ~num;
                this.AddListItem(presetList, preset, user).transform.SetSiblingIndex(index);
                presetList.presetList.ScrollToElement(index, true, false);
                presetList.presetList.SetBuyButtonEnabled(false);
            }
            if (this.IsMaxCount(presetList, storey.marketPreset))
            {
                int index = GetListItems(presetList).FindLastIndex(new Predicate<PresetListItemComponent>(storey.<>m__0));
                if (index != -1)
                {
                    presetList.presetList.RemoveElement(index);
                }
            }
            this.UpdateMarketItemsNames(presetList);
        }

        private void ChangeSelectedPreset(Optional<SingleNode<SelectedPresetComponent>> selectedPreset, Entity preset)
        {
            if (selectedPreset.IsPresent())
            {
                selectedPreset.Get().Entity.RemoveComponent<SelectedPresetComponent>();
            }
            preset.AddComponent<SelectedPresetComponent>();
        }

        [OnEventFire]
        public void ClearList(NodeRemoveEvent e, PresetListNode presetList)
        {
            presetList.presetList.Clear();
            presetList.presetList.SetBuyButtonEnabled(false);
        }

        [OnEventFire]
        public void ControlRentThingOnScreen(NodeAddedEvent e, NotOwnedSelectedPreset preset, SelfUserNode selfUser, SingleNode<ElementsToChangeWhenRentedTankComponent> helper, SingleNode<PresetListComponent> presetsScreen)
        {
            helper.component.ReturnScreenToNormalState();
        }

        [OnEventFire]
        public void ControlRentThingOnScreen(NodeAddedEvent e, SelectedPresetNode preset, SelfUserNode selfUser, SingleNode<ElementsToChangeWhenRentedTankComponent> helper, SingleNode<PresetListComponent> presetsScreen)
        {
            if (preset.userGroup.Key != selfUser.userGroup.Key)
            {
                helper.component.SetScreenToRentedTankState();
            }
            else
            {
                helper.component.ReturnScreenToNormalState();
            }
        }

        [OnEventFire]
        public void ControlRentThingOnScreen(NodeRemoveEvent e, SelectedPresetNode preset, [JoinAll] SelfUserNode selfUser, [JoinAll] SingleNode<ElementsToChangeWhenRentedTankComponent> helper, [JoinAll] SingleNode<PresetListComponent> presetsScreen)
        {
            helper.component.ReturnScreenToNormalState();
        }

        private static PresetListItemComponent GetListItem(PresetListNode presetList, int index) => 
            presetList.presetList.ContentRoot.GetChild(index).GetComponent<PresetListItemComponent>();

        private static List<PresetListItemComponent> GetListItems(PresetListNode presetList)
        {
            RectTransform contentRoot = presetList.presetList.ContentRoot;
            List<PresetListItemComponent> list = new List<PresetListItemComponent>(contentRoot.childCount);
            for (int i = 0; i < contentRoot.childCount; i++)
            {
                PresetListItemComponent item = contentRoot.GetChild(i).GetComponent<PresetListItemComponent>();
                list.Add(item);
            }
            return list;
        }

        private static List<Entity> GetListPresets(PresetListNode presetList)
        {
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = item => item.Preset;
            }
            return GetListItems(presetList).ConvertAll<Entity>(<>f__am$cache2);
        }

        private string GetPresetName(Entity preset)
        {
            GetPresetNameEvent eventInstance = new GetPresetNameEvent();
            base.ScheduleEvent(eventInstance, preset);
            return eventInstance.Name;
        }

        [OnEventFire]
        public void InitList(NodeAddedEvent e, PresetListNode presetList, [JoinAll] ICollection<BasePresetNode> presets, [JoinAll] SelfUserNode selfUser, [JoinAll] MarketPresetNode marketPreset)
        {
            List<BasePresetNode> list = new List<BasePresetNode>(presets);
            list.Sort(nodeComparer);
            int elementIndex = 0;
            for (int i = 0; i < list.Count; i++)
            {
                BasePresetNode preset = list[i];
                if (preset.Entity.HasComponent<CreateByRankConfigComponent>())
                {
                    foreach (int num3 in preset.Entity.GetComponent<CreateByRankConfigComponent>().UserRankListToCreateItem)
                    {
                        if (selfUser.userRank.Rank < num3)
                        {
                            this.AddListItem(presetList, preset, selfUser).Rank = num3;
                        }
                    }
                }
                if (!preset.Entity.HasComponent<ItemsBuyCountLimitComponent>() || !this.IsMaxCount(presetList, marketPreset))
                {
                    this.AddListItem(presetList, preset, selfUser);
                    if (preset.Entity.HasComponent<MountedItemComponent>() && preset.Entity.HasComponent<SelectedPresetComponent>())
                    {
                        elementIndex = i;
                    }
                }
            }
            this.UpdateMarketItemsNames(presetList);
            presetList.presetList.ScrollToElement(elementIndex, true, false);
        }

        [OnEventFire]
        public void InitName(NodeAddedEvent e, SingleNode<PresetLabelComponent> presetNameLabel, SelectedPresetNode preset, SelfUserNode selfUser)
        {
            string presetName = this.GetPresetName(preset.Entity);
            presetNameLabel.component.PresetName = presetName;
        }

        [OnEventFire]
        public void InitName(NodeAddedEvent e, SingleNode<PresetNameEditorComponent> presetNameEditor, SelfUserNode selfUser, SelectedPresetNode preset)
        {
            string presetName = this.GetPresetName(preset.Entity);
            presetNameEditor.component.PresetName = presetName;
        }

        [OnEventFire]
        public void InitSelectedPreset(NodeAddedEvent e, MountedPresetNode presetNode, [JoinAll] Optional<SingleNode<SelectedPresetComponent>> selectedPreset)
        {
            if (!selectedPreset.IsPresent() && !presetNode.Entity.HasComponent<SelectedPresetComponent>())
            {
                presetNode.Entity.AddComponent<SelectedPresetComponent>();
                base.ScheduleEvent(new ResetPreviewEvent(), presetNode);
            }
        }

        [OnEventFire]
        public void InitSelectedPreset(NodeAddedEvent e, SelfUserWithPrototypeNode userWithRentedPreset, [Context, Combine] MountedPresetNode presetNode, [JoinAll] Optional<SingleNode<SelectedPresetComponent>> selectedPreset)
        {
            if (presetNode.Entity.Equals(userWithRentedPreset.userUseItemsPrototype.Preset))
            {
                if (selectedPreset.IsPresent())
                {
                    selectedPreset.Get().Entity.RemoveComponent<SelectedPresetComponent>();
                }
                if (!presetNode.Entity.HasComponent<SelectedPresetComponent>())
                {
                    presetNode.Entity.AddComponent<SelectedPresetComponent>();
                    base.ScheduleEvent(new ResetPreviewEvent(), presetNode);
                }
            }
        }

        [OnEventFire]
        public void InitSelectedPreset(NodeRemoveEvent e, SelfUserWithPrototypeNode user, [JoinByUser] MountedPresetNode presetNode, [JoinAll] Optional<SingleNode<SelectedPresetComponent>> selectedPreset)
        {
            if (!selectedPreset.IsPresent() && !presetNode.Entity.HasComponent<SelectedPresetComponent>())
            {
                presetNode.Entity.AddComponent<SelectedPresetComponent>();
                base.ScheduleEvent(new ResetPreviewEvent(), presetNode);
            }
        }

        private bool IsMaxCount(PresetListNode presetList, MarketPresetNode marketPreset)
        {
            int num = !marketPreset.Entity.HasComponent<ItemsBuyCountLimitComponent>() ? 0x7fffffff : marketPreset.Entity.GetComponent<ItemsBuyCountLimitComponent>().Limit;
            <>f__am$cache0 ??= item => (item.IsOwned && !item.Preset.HasComponent<CreatedByRankItemComponent>());
            return (GetListItems(presetList).Count<PresetListItemComponent>(<>f__am$cache0) >= num);
        }

        [OnEventFire]
        public void MountPreset(ListItemSelectedEvent e, ListItemNode listItem)
        {
            Entity preset = listItem.presetListItem.Preset;
            if (preset != null)
            {
                base.ScheduleEvent(new MountPresetEvent(), preset);
            }
        }

        [OnEventFire]
        public void MountPreset(MountPresetEvent e, BasePresetNode presetNode, [JoinAll] SelfUserNode selfUser, [JoinAll] Optional<SingleNode<SelectedPresetComponent>> selectedPreset, [JoinAll] ICollection<MountedPresetNode> mountedPresets)
        {
            <MountPreset>c__AnonStorey0 storey = new <MountPreset>c__AnonStorey0 {
                selfUser = selfUser
            };
            Entity entity = presetNode.Entity;
            if (!entity.HasComponent<UserItemComponent>())
            {
                MountedPresetNode node = mountedPresets.First<MountedPresetNode>(new Func<MountedPresetNode, bool>(storey.<>m__0));
                this.ChangeSelectedPreset(selectedPreset, node.Entity);
            }
            if (entity.HasComponent<UserItemComponent>() && (entity.GetComponent<UserGroupComponent>().Key == storey.selfUser.userGroup.Key))
            {
                if (!entity.HasComponent<MountedItemComponent>())
                {
                    base.ScheduleEvent<MountItemEvent>(entity);
                }
                this.ChangeSelectedPreset(selectedPreset, entity);
            }
            if (!entity.HasComponent<UserGroupComponent>() || (entity.GetComponent<UserGroupComponent>().Key == storey.selfUser.userGroup.Key))
            {
                base.ScheduleEvent(new RemoveRentedPresetEvent(), storey.selfUser);
            }
            else
            {
                base.NewEvent(new MountRentedPresetEvent()).Attach(entity).Attach(storey.selfUser).Schedule();
                this.ChangeSelectedPreset(selectedPreset, entity);
            }
        }

        [OnEventFire]
        public void ProvidePresetName(GetPresetNameEvent e, UserPresetNode preset, [JoinAll] SelfUserNode selfUser)
        {
            if (preset.userGroup.Key == selfUser.userGroup.Key)
            {
                string name = preset.presetName.Name;
                e.Name = name;
            }
            else
            {
                IList<OfferNode> source = base.Select<OfferNode, SpecialOfferGroupComponent>(preset.Entity);
                e.Name = !source.Any<OfferNode>() ? preset.presetName.Name : source.First<OfferNode>().legendaryTankSpecialOffer.TankRole.ToString();
            }
        }

        [OnEventFire]
        public void RemoveListItem(NodeRemoveEvent e, BasePresetNode preset, [JoinAll] PresetListNode presetList)
        {
            int index = GetListPresets(presetList).BinarySearch(preset.Entity, comparer);
            if (index >= 0)
            {
                presetList.presetList.RemoveElement(index);
            }
            this.UpdateMarketItemsNames(presetList);
        }

        [OnEventFire]
        public void RemoveLockedByRankItem(UpdateRankEvent e, SelfUserNode user, [JoinAll] PresetListNode presetList)
        {
            int rank = user.userRank.Rank;
            List<PresetListItemComponent> listItems = GetListItems(presetList);
            for (int i = 0; i < listItems.Count; i++)
            {
                PresetListItemComponent component = listItems[i];
                if (component.Preset.HasComponent<CreateByRankConfigComponent>() && ((rank > component.Rank) && (component.Rank > 0)))
                {
                    presetList.presetList.RemoveElement(i);
                }
            }
            this.UpdateMarketItemsNames(presetList);
        }

        [OnEventFire]
        public void ResetPreviewOnOfferRemove(NodeRemoveEvent e, SelfUserWithPrototypeNode userWithRentedPreset, [JoinAll] ICollection<MountedPresetNode> mountedPresets, [JoinAll] SelfUserNode selfUser, [JoinAll] Optional<SelectedPresetNode> selectedPreset, [JoinAll] Optional<PresetListNode> presetList)
        {
            <ResetPreviewOnOfferRemove>c__AnonStorey4 storey = new <ResetPreviewOnOfferRemove>c__AnonStorey4 {
                selfUser = selfUser
            };
            if ((!selectedPreset.IsPresent() || userWithRentedPreset.userUseItemsPrototype.Preset.Equals(selectedPreset.Get().Entity)) && !presetList.IsPresent())
            {
                if (selectedPreset.IsPresent())
                {
                    selectedPreset.Get().Entity.RemoveComponent<SelectedPresetComponent>();
                }
                IEnumerable<MountedPresetNode> source = mountedPresets.Where<MountedPresetNode>(new Func<MountedPresetNode, bool>(storey.<>m__0));
                if (source.Count<MountedPresetNode>() != 0)
                {
                    source.First<MountedPresetNode>().Entity.AddComponent<SelectedPresetComponent>();
                    base.ScheduleEvent<ResetPreviewEvent>(userWithRentedPreset);
                }
            }
        }

        [OnEventFire]
        public void SaveName(PresetNameChangedEvent e, SingleNode<PresetNameEditorComponent> presetNameEditor, [JoinAll] SelfUserNode selfUser, [JoinByUser, Mandatory] MountedPresetNode preset, [JoinAll] PresetListNode presetList)
        {
            <SaveName>c__AnonStorey2 storey = new <SaveName>c__AnonStorey2 {
                preset = preset
            };
            string presetName = presetNameEditor.component.PresetName;
            storey.preset.presetName.Name = presetName;
            storey.preset.presetName.OnChange();
            GetListItems(presetList).Find(new Predicate<PresetListItemComponent>(storey.<>m__0)).PresetName = presetName;
        }

        [OnEventFire]
        public void ScrollList(MountPresetEvent e, BasePresetNode preset, [JoinAll] PresetListNode presetList, [JoinAll] ICollection<ListItemNode> listItems)
        {
            <ScrollList>c__AnonStorey1 storey = new <ScrollList>c__AnonStorey1 {
                preset = preset
            };
            List<Entity> listPresets = GetListPresets(presetList);
            ListItemNode node = listItems.First<ListItemNode>(new Func<ListItemNode, bool>(storey.<>m__0));
            bool enabled = false;
            bool flag2 = false;
            if (!node.presetListItem.IsUserItem)
            {
                enabled = node.presetListItem.Rank > 0;
                flag2 = GarageItemsRegistry.GetItem<GarageItem>(storey.preset.Entity).IsBuyable && !enabled;
            }
            presetList.presetList.SetBuyButtonEnabled(flag2);
            presetList.presetList.SetLockedByRankTextEnabled(enabled);
            if (flag2)
            {
                GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(storey.preset.Entity);
                presetList.presetList.XBuyPrice.SetPrice(item.OldXPrice, item.XPrice);
            }
            if (enabled)
            {
                presetList.presetList.LockedByRankText = string.Format((string) presetList.presetList.LockedByRankLocalizedText, node.presetListItem.Rank);
            }
        }

        [OnEventFire]
        public void SetBuyButton(ListItemSelectedEvent e, ListItemNode listItem, [JoinAll] PresetListNode presetList)
        {
            Entity preset = listItem.presetListItem.Preset;
            if (preset != null)
            {
                bool enabled = false;
                bool flag2 = false;
                if (!listItem.presetListItem.IsUserItem)
                {
                    enabled = listItem.presetListItem.Rank > 0;
                    flag2 = GarageItemsRegistry.GetItem<GarageItem>(preset).IsBuyable && !enabled;
                }
                presetList.presetList.SetBuyButtonEnabled(flag2);
                presetList.presetList.SetLockedByRankTextEnabled(enabled);
                if (flag2)
                {
                    GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(preset);
                    int xPrice = item.XPrice;
                    presetList.presetList.XBuyPrice.SetPrice(item.OldXPrice, xPrice);
                }
                if (enabled)
                {
                    presetList.presetList.LockedByRankText = string.Format((string) presetList.presetList.LockedByRankLocalizedText, listItem.presetListItem.Rank);
                }
            }
        }

        [OnEventFire]
        public void ShowBuyDialog(ButtonClickEvent e, BuyButtonNode buyButton, [JoinAll] PresetListNode presetList)
        {
            int selectedItemIndex = presetList.presetList.SelectedItemIndex;
            PresetListItemComponent listItem = GetListItem(presetList, selectedItemIndex);
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(listItem.Preset);
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = delegate {
                };
            }
            presetList.presetList.BuyDialog.XShow(item, <>f__am$cache1, item.XPrice, 1, null, false, null);
        }

        private void UpdateMarketItemsNames(PresetListNode presetList)
        {
            List<PresetListItemComponent> listItems = GetListItems(presetList);
            for (int i = 0; i < listItems.Count; i++)
            {
                PresetListItemComponent component = listItems[i];
                if (!component.IsUserItem)
                {
                    component.PresetName = component.Preset.GetComponent<DescriptionItemComponent>().Name + " " + (i + 1);
                }
            }
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        [CompilerGenerated]
        private sealed class <AddListItemOnNewPreset>c__AnonStorey3
        {
            internal PresetSystem.MarketPresetNode marketPreset;

            internal bool <>m__0(PresetListItemComponent i) => 
                this.marketPreset.Entity.Equals(i.Preset);
        }

        [CompilerGenerated]
        private sealed class <MountPreset>c__AnonStorey0
        {
            internal PresetSystem.SelfUserNode selfUser;

            internal bool <>m__0(PresetSystem.MountedPresetNode it) => 
                it.userGroup.Key == this.selfUser.userGroup.Key;
        }

        [CompilerGenerated]
        private sealed class <ResetPreviewOnOfferRemove>c__AnonStorey4
        {
            internal PresetSystem.SelfUserNode selfUser;

            internal bool <>m__0(PresetSystem.MountedPresetNode it) => 
                it.userGroup.Key == this.selfUser.userGroup.Key;
        }

        [CompilerGenerated]
        private sealed class <SaveName>c__AnonStorey2
        {
            internal PresetSystem.MountedPresetNode preset;

            internal bool <>m__0(PresetListItemComponent item) => 
                ReferenceEquals(item.Preset, this.preset.Entity);
        }

        [CompilerGenerated]
        private sealed class <ScrollList>c__AnonStorey1
        {
            internal PresetSystem.BasePresetNode preset;

            internal bool <>m__0(PresetSystem.ListItemNode it) => 
                it.presetListItem.Preset.Equals(this.preset);
        }

        public class BasePresetNode : Node
        {
            public PresetItemComponent presetItem;
        }

        public class BuyButtonNode : Node
        {
            public PresetBuyButtonComponent presetBuyButton;
        }

        public class ListItemNode : Node
        {
            public PresetListItemComponent presetListItem;
        }

        public class MarketPresetNode : PresetSystem.BasePresetNode
        {
            public MarketItemComponent marketItem;
        }

        public class MountedPresetNode : PresetSystem.UserPresetNode
        {
            public MountedItemComponent mountedItem;
        }

        [Not(typeof(UserGroupComponent))]
        public class NotOwnedSelectedPreset : PresetSystem.SelectedPresetNode
        {
        }

        public class OfferNode : Node
        {
            public LegendaryTankSpecialOfferComponent legendaryTankSpecialOffer;
        }

        public class PresetComparer : IComparer<Entity>
        {
            public int Compare(Entity p1, Entity p2)
            {
                int num = !p1.HasComponent<UserItemComponent>() ? 0 : 1;
                int num2 = !p2.HasComponent<UserItemComponent>() ? 0 : 1;
                if (num != num2)
                {
                    return (num2 - num);
                }
                long id = SelfUserComponent.SelfUser.Id;
                if (p1.HasComponent<UserItemComponent>())
                {
                    long key = p2.GetComponent<UserGroupComponent>().Key;
                    int num6 = (p1.GetComponent<UserGroupComponent>().Key != id) ? 0 : 1;
                    int num7 = (key != id) ? 0 : 1;
                    if (num6 != num7)
                    {
                        return (num6 - num7);
                    }
                }
                return p1.Id.CompareTo(p2.Id);
            }
        }

        public class PresetListNode : Node
        {
            public PresetListComponent presetList;
        }

        private class PresetNodeComparer : IComparer<PresetSystem.BasePresetNode>
        {
            public int Compare(PresetSystem.BasePresetNode p1, PresetSystem.BasePresetNode p2) => 
                PresetSystem.comparer.Compare(p1.Entity, p2.Entity);
        }

        public class SelectedPresetNode : PresetSystem.MountedPresetNode
        {
            public SelectedPresetComponent selectedPreset;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserRankComponent userRank;
            public UserGroupComponent userGroup;
        }

        public class SelfUserWithPrototypeNode : PresetSystem.SelfUserNode
        {
            public UserUseItemsPrototypeComponent userUseItemsPrototype;
        }

        public class UserPresetNode : PresetSystem.BasePresetNode
        {
            public PresetNameComponent presetName;
            public UserGroupComponent userGroup;
        }
    }
}

