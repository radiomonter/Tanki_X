namespace Tanks.Battle.ClientHUD.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientGarage.API;

    public class InventoryHUDSystem : ECSSystem
    {
        [OnEventFire]
        public void CheckInventoryHUDNecessity(CheckInventoryHUDNecessityEvent e, HUDNodes.SelfBattleUserAsTankNode selfUser, [JoinByUser, Combine] SlotNode slot, [JoinByModule] ModuleNode module)
        {
            IList<ModuleUsesCounterNode> source = base.Select<ModuleUsesCounterNode>(module.Entity, typeof(ModuleGroupComponent));
            bool flag = (source.Count == 0) || ((source.Count > 0) && (source.First<ModuleUsesCounterNode>().userItemCounter.Count > 0L));
            e.Necessity = e.Necessity || flag;
        }

        [OnEventFire]
        public void InitModule(NodeAddedEvent e, [Context, Combine] ModuleNode module, [Context, JoinByModule] SlotNode slot, [Context, JoinByModule] ModuleHUDNode hud, [JoinAll] SingleNode<InventoryHUDComponent> inventory)
        {
            hud.itemButton.Icon = module.itemIcon.SpriteUid;
            hud.itemButton.KeyBind = inventory.component.GetKeyBindForItem(hud.itemButton);
            hud.itemButton.MaxItemAmmunitionCount = slot.inventoryAmmunition.MaxCount;
            hud.itemButton.ItemAmmunitionCount = slot.inventoryAmmunition.CurrentCount;
        }

        [OnEventFire]
        public void InitSlots(NodeAddedEvent e, SingleNode<InventoryHUDComponent> inventory, HUDNodes.SelfBattleUserAsTankNode selfUser, [JoinByUser] ICollection<ModuleNode> modules)
        {
            CheckInventoryHUDNecessityEvent eventInstance = new CheckInventoryHUDNecessityEvent();
            base.ScheduleEvent(eventInstance, selfUser);
            if (eventInstance.Necessity)
            {
                base.ScheduleEvent<InitSlotsEvent>(selfUser);
            }
        }

        [OnEventFire]
        public void InitSlots(InitSlotsEvent e, HUDNodes.SelfBattleUserAsTankNode selfUser, [JoinByUser, Combine] SlotNode slot, [JoinByModule] ModuleNode module, [JoinByModule] ICollection<ModuleHUDNode> moduleHuds, [JoinAll] SingleNode<InventoryHUDComponent> inventory)
        {
            if (moduleHuds.Count <= 0)
            {
                EntityBehaviour behaviour = inventory.component.CreateModule(slot.slotUserItemInfo.Slot);
                module.moduleGroup.Attach(behaviour.Entity);
                if (module.Entity.HasComponent<GoldBonusModuleItemComponent>())
                {
                    inventory.component.CreateGoldBonusesCounter(behaviour);
                }
            }
        }

        public class CheckInventoryHUDNecessityEvent : Event
        {
            public CheckInventoryHUDNecessityEvent()
            {
                this.Necessity = false;
            }

            public bool Necessity { get; set; }
        }

        public class InitSlotsEvent : Event
        {
        }

        public class ModuleHUDNode : Node
        {
            public ModuleGroupComponent moduleGroup;
            public ItemButtonComponent itemButton;
        }

        public class ModuleNode : Node
        {
            public ModuleGroupComponent moduleGroup;
            public ModuleItemComponent moduleItem;
            public UserGroupComponent userGroup;
            public ItemIconComponent itemIcon;
        }

        public class ModuleUsesCounterNode : Node
        {
            public UserItemComponent userItem;
            public UserGroupComponent userGroup;
            public ModuleUsesCounterComponent moduleUsesCounter;
            public UserItemCounterComponent userItemCounter;
        }

        public class SlotNode : Node
        {
            public SlotUserItemInfoComponent slotUserItemInfo;
            public ModuleGroupComponent moduleGroup;
            public TankGroupComponent tankGroup;
            public UserGroupComponent userGroup;
            public InventoryAmmunitionComponent inventoryAmmunition;
        }
    }
}

