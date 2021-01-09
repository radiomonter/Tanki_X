﻿namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;

    public class ModuleSystem : ECSSystem
    {
        [OnEventFire]
        public void BuildModuleUpgradePropertiesInfo(NodeAddedEvent e, ModuleMarketItemUpgradeInfoNode moduleMarketItem)
        {
            Entity entity = base.CreateEntity(moduleMarketItem.moduleUpgradePropertiesInfo.Template.TemplateId, moduleMarketItem.moduleUpgradePropertiesInfo.Path);
            moduleMarketItem.marketItemGroup.Attach(entity);
        }

        [OnEventFire]
        public void OnModuleAssembled(ModuleAssembledEvent e, ModuleNode node)
        {
            base.ScheduleEvent<ModuleChangedEvent>(node);
        }

        [OnEventFire]
        public void OnModuleUpgraded(ModuleUpgradedEvent e, ModuleNode node)
        {
            base.ScheduleEvent<ModuleChangedEvent>(node);
        }

        [OnEventComplete]
        public void OnMountModule(NodeAddedEvent e, MountedModuleNode item, [JoinByUser] SingleNode<SelfUserComponent> user)
        {
            base.ScheduleEvent<ModuleChangedEvent>(item);
        }

        [OnEventComplete]
        public void OnUnmountModule(NodeRemoveEvent e, SlotNode slot, [Context, JoinByModule] ModuleNode item, [JoinByUser] SingleNode<SelfUserComponent> user)
        {
            base.ScheduleEvent<ModuleChangedEvent>(item);
        }

        public class ModuleMarketItemUpgradeInfoNode : Node
        {
            public MarketItemGroupComponent marketItemGroup;
            public MarketItemComponent marketItem;
            public ModuleItemComponent moduleItem;
            public ModuleUpgradePropertiesInfoComponent moduleUpgradePropertiesInfo;
        }

        public class ModuleNode : Node
        {
            public SelfComponent self;
            public UserItemComponent userItem;
            public ModuleItemComponent moduleItem;
        }

        public class MountedModuleNode : ModuleSystem.ModuleNode
        {
            public MountedItemComponent mountedItem;
        }

        public class SlotNode : Node
        {
            public UserItemComponent userItem;
            public SlotUserItemInfoComponent slotUserItemInfo;
            public SlotTankPartComponent slotTankPart;
        }
    }
}

