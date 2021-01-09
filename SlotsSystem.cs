using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;

public class SlotsSystem : ECSSystem
{
    [CompilerGenerated]
    private static Func<NotReservedSlotNode, bool> <>f__am$cache0;
    [CompilerGenerated]
    private static Func<NotReservedSlotNode, bool> <>f__am$cache1;

    private void InitSlotUI(SlotUIComponent slotUI, NotReservedSlotNode notReservedSlotNode)
    {
        if (slotUI != null)
        {
            slotUI.Locked = false;
            slotUI.Rank = notReservedSlotNode.slotUserItemInfo.UpgradeLevel;
            slotUI.ModuleIcon.color = Color.white;
            slotUI.Slot = notReservedSlotNode.slotUserItemInfo.Slot;
            slotUI.TankPart = notReservedSlotNode.slotTankPart.TankPart;
            if (slotUI.SelectionImage != null)
            {
                slotUI.SelectionImage.color = Color.white;
            }
            if (notReservedSlotNode.Entity.HasComponent<SlotUIComponent>())
            {
                notReservedSlotNode.Entity.RemoveComponent<SlotUIComponent>();
            }
            notReservedSlotNode.Entity.AddComponent(slotUI);
        }
    }

    private void InitSlotUIToggle(SlotUIComponent slotUI, Entity slotEntity)
    {
        ToggleListItemComponent component = slotUI.GetComponent<ToggleListItemComponent>();
        if (slotEntity.HasComponent<ToggleListItemComponent>())
        {
            slotEntity.RemoveComponent<ToggleListItemComponent>();
        }
        slotEntity.AddComponent(component);
    }

    [OnEventFire]
    public void LockSlot(NodeAddedEvent e, [Combine] LockedSlotNode slot, [Context, JoinByUser] SelectedPresetNode selectedPreset)
    {
        slot.slotUI.Locked = true;
    }

    [OnEventFire]
    public void OnModuleWasUpgraded(ModuleUpgradedEvent e, UserModuleNode userModule, [JoinByModule] SlotWithUIAndModuleNode selectedSlot, UserModuleNode userModule2, [JoinByParentGroup] Optional<ModuleCardNode> moduleCard)
    {
        int level = (int) userModule.moduleUpgradeLevel.Level;
        if (level == userModule.moduleCardsComposition.UpgradePrices.Count)
        {
            selectedSlot.slotUI.UpgradeIcon.gameObject.SetActive(false);
        }
        else if (!moduleCard.IsPresent())
        {
            selectedSlot.slotUI.UpgradeIcon.gameObject.SetActive(false);
        }
        else if (userModule.moduleCardsComposition.UpgradePrices[level].Cards > moduleCard.Get().userItemCounter.Count)
        {
            selectedSlot.slotUI.UpgradeIcon.gameObject.SetActive(false);
        }
    }

    [OnEventFire]
    public void RemoveUpgradeIcon(NodeRemoveEvent e, SlotWithUIAndModuleNode userModule)
    {
        userModule.slotUI.UpgradeIcon.gameObject.SetActive(false);
    }

    [OnEventFire]
    public void ResetIcon(NodeRemoveEvent e, SlotWithUIAndModuleNode slot)
    {
        slot.slotUI.ModuleIconImageSkin.SpriteUid = null;
    }

    [OnEventFire]
    public void SetIcon(NodeAddedEvent e, SlotWithUIAndModuleNode slot, [JoinByModule] ModuleNode module)
    {
        slot.slotUI.ModuleIconImageSkin.SpriteUid = module.itemBigIcon.SpriteUid;
    }

    [OnEventFire]
    public void SetSettingsSlotIcons(NodeAddedEvent e, SingleNode<SettingsSlotsUIComponent> slotsUI, [JoinAll] ICollection<SlotNode> slots, [JoinAll, Context] SelectedPresetNode selectedPreset)
    {
        foreach (SlotNode node in slots)
        {
            SettingsSlotUIComponent slot = slotsUI.component.GetSlot(node.slotUserItemInfo.Slot);
            if (slot != null)
            {
                string udid = string.Empty;
                if ((node.Entity.GetComponent<UserGroupComponent>().Key == selectedPreset.userGroup.Key) || node.Entity.HasComponent<SlotReservedComponent>())
                {
                    IList<ModuleNode> list = base.Select<ModuleNode>(node.Entity, typeof(ModuleGroupComponent));
                    udid = (list.Count <= 0) ? string.Empty : list[0].itemBigIcon.SpriteUid;
                    bool moduleActive = false;
                    if (list.Count > 0)
                    {
                        IList<ModuleUsesCounterNode> source = base.Select<ModuleUsesCounterNode>(list[0].Entity, typeof(ModuleGroupComponent));
                        moduleActive = (source.Count == 0) || ((source.Count > 0) && (source.First<ModuleUsesCounterNode>().userItemCounter.Count > 0L));
                    }
                    slot.SetIcon(udid, moduleActive);
                }
            }
        }
    }

    [OnEventComplete]
    public void ShowUpgradeIcon(NodeAddedEvent e, SlotWithUIAndModuleNode selectedSlot, [JoinByModule] Optional<UserModuleNode> userModule, [JoinByParentGroup] Optional<ModuleCardNode> moduleCard, [JoinAll] SelfUserNode selfUser)
    {
        if (!userModule.IsPresent() || (userModule.Get().userGroup.Key == selfUser.userGroup.Key))
        {
            selectedSlot.slotUI.UpgradeIcon.gameObject.SetActive(false);
            if (moduleCard.IsPresent())
            {
                long count = moduleCard.Get().userItemCounter.Count;
                if (userModule.IsPresent())
                {
                    long num2 = userModule.Get().moduleUpgradeLevel.Level + 1L;
                    if ((num2 <= userModule.Get().moduleCardsComposition.UpgradePrices.Count) && (userModule.Get().moduleCardsComposition.UpgradePrices[(int) (num2 - 1L)].Cards <= count))
                    {
                        selectedSlot.slotUI.UpgradeIcon.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    [OnEventFire]
    public void SlotsInGarageInited(NodeAddedEvent e, SingleNode<GarageSlotsUIPanelComponent> screen, [Context] UserNode user, [Context] ICollection<NotReservedSlotNode> slots, [Context] SelectedPresetNode selectedPreset)
    {
        <SlotsInGarageInited>c__AnonStorey0 storey = new <SlotsInGarageInited>c__AnonStorey0 {
            selectedPreset = selectedPreset
        };
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = it => it.Entity.HasComponent<SlotUIComponent>();
        }
        foreach (NotReservedSlotNode node in slots.Where<NotReservedSlotNode>(<>f__am$cache1))
        {
            node.Entity.RemoveComponent<SlotUIComponent>();
        }
        foreach (NotReservedSlotNode node2 in slots.Where<NotReservedSlotNode>(new Func<NotReservedSlotNode, bool>(storey.<>m__0)))
        {
            SlotUIComponent slot = screen.component.GetSlot(node2.slotUserItemInfo.Slot);
            this.InitSlotUI(slot, node2);
        }
    }

    [OnEventFire]
    public void SlotsInModulesScreenInit(NodeAddedEvent e, SingleNode<ModulesScreenUIComponent> screen, UserWithoutRentedPreset user, [JoinByUser] ICollection<NotReservedSlotNode> slots)
    {
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = it => it.Entity.HasComponent<SlotUIComponent>();
        }
        foreach (NotReservedSlotNode node in slots.Where<NotReservedSlotNode>(<>f__am$cache0))
        {
            node.Entity.RemoveComponent<SlotUIComponent>();
        }
        foreach (NotReservedSlotNode node2 in slots)
        {
            SlotUIComponent slot = screen.component.GetSlot(node2.slotUserItemInfo.Slot);
            this.InitSlotUI(slot, node2);
            this.InitSlotUIToggle(slot, node2.Entity);
        }
    }

    [OnEventFire]
    public void UnlockSlot(NodeRemoveEvent e, [Combine] LockedSlotNode slot, [Context, JoinByUser] SelectedPresetNode selectedPreset)
    {
        slot.slotUI.Locked = false;
        slot.slotUI.Rank = slot.slotUserItemInfo.UpgradeLevel;
    }

    [CompilerGenerated]
    private sealed class <SlotsInGarageInited>c__AnonStorey0
    {
        internal SlotsSystem.SelectedPresetNode selectedPreset;

        internal bool <>m__0(SlotsSystem.NotReservedSlotNode it) => 
            it.Entity.GetComponent<UserGroupComponent>().Key == this.selectedPreset.userGroup.Key;
    }

    public class LockedSlotNode : SlotsSystem.NotReservedSlotNode
    {
        public SlotLockedComponent slotLocked;
        public SlotUIComponent slotUI;
    }

    public class ModuleCardNode : Node
    {
        public ModuleCardItemComponent moduleCardItem;
        public UserItemComponent userItem;
        public UserItemCounterComponent userItemCounter;
    }

    public class ModuleNode : Node
    {
        public ModuleItemComponent moduleItem;
        public DescriptionItemComponent descriptionItem;
        public ModuleGroupComponent moduleGroup;
        public ItemIconComponent itemIcon;
        public ItemBigIconComponent itemBigIcon;
    }

    public class ModuleUsesCounterNode : Node
    {
        public UserItemComponent userItem;
        public UserGroupComponent userGroup;
        public ModuleUsesCounterComponent moduleUsesCounter;
        public UserItemCounterComponent userItemCounter;
    }

    [Not(typeof(SlotReservedComponent))]
    public class NotReservedSlotNode : SlotsSystem.SlotNode
    {
    }

    public class SelectedPresetNode : Node
    {
        public SelectedPresetComponent selectedPreset;
        public UserGroupComponent userGroup;
    }

    public class SelfUserNode : Node
    {
        public SelfUserComponent selfUser;
        public UserGroupComponent userGroup;
    }

    public class SlotNode : Node
    {
        public SlotUserItemInfoComponent slotUserItemInfo;
        public SlotTankPartComponent slotTankPart;
    }

    public class SlotWithUIAndModuleNode : Node
    {
        public SlotUserItemInfoComponent slotUserItemInfo;
        public SlotUIComponent slotUI;
        public ModuleGroupComponent moduleGroup;
    }

    public class UserModuleNode : SlotsSystem.ModuleNode
    {
        public UserItemComponent userItem;
        public ModuleUpgradeLevelComponent moduleUpgradeLevel;
        public ModuleCardsCompositionComponent moduleCardsComposition;
        public UserGroupComponent userGroup;
    }

    public class UserNode : Node
    {
        public UserGroupComponent userGroup;
        public SelfUserComponent selfUser;
    }

    [Not(typeof(UserUseItemsPrototypeComponent))]
    public class UserWithoutRentedPreset : SlotsSystem.SelfUserNode
    {
    }
}

