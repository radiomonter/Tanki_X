namespace Tanks.Battle.ClientHUD.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientGarage.API;

    public class InventorySlotActivationSystem : ECSSystem
    {
        private readonly Dictionary<string, Slot> actionToSlotMap;

        public InventorySlotActivationSystem()
        {
            Dictionary<string, Slot> dictionary = new Dictionary<string, Slot> {
                { 
                    InventoryAction.INVENTORY_SLOT1,
                    Slot.SLOT1
                },
                { 
                    InventoryAction.INVENTORY_SLOT3,
                    Slot.SLOT2
                },
                { 
                    InventoryAction.INVENTORY_SLOT5,
                    Slot.SLOT3
                },
                { 
                    InventoryAction.INVENTORY_SLOT2,
                    Slot.SLOT4
                },
                { 
                    InventoryAction.INVENTORY_SLOT4,
                    Slot.SLOT5
                },
                { 
                    InventoryAction.INVENTORY_SLOT6,
                    Slot.SLOT6
                },
                { 
                    InventoryAction.INVENTORY_GOLDBOX,
                    Slot.SLOT7
                }
            };
            this.actionToSlotMap = dictionary;
        }

        [OnEventFire]
        public void TryActivate(UpdateEvent e, HUDNodes.SelfBattleUserAsTankNode self, [JoinByUser, Combine] HUDNodes.Modules.SlotWithModuleNode slot, [JoinByModule] SingleNode<ItemButtonComponent> hud, [JoinByModule] NotAutostartModuleNode module)
        {
            if (slot.Entity.HasComponent<InventoryEnabledStateComponent>())
            {
                if (slot.Entity.HasComponent<InventorySlotTemporaryBlockedComponent>())
                {
                    foreach (KeyValuePair<string, Slot> pair in this.actionToSlotMap)
                    {
                        if (InputManager.GetActionKeyDown(pair.Key) && (slot.slotUserItemInfo.Slot == ((Slot) pair.Value)))
                        {
                            hud.component.PressedWhenDisable();
                        }
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, Slot> pair2 in this.actionToSlotMap)
                    {
                        if (InputManager.GetActionKeyDown(pair2.Key) && (slot.slotUserItemInfo.Slot == ((Slot) pair2.Value)))
                        {
                            hud.component.Activate();
                            base.ScheduleEvent<ActivateModuleEvent>(slot);
                        }
                    }
                }
            }
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        [Not(typeof(PassiveModuleComponent))]
        public class NotAutostartModuleNode : Node
        {
            public ModuleItemComponent moduleItem;
            public UserItemComponent userItem;
            public ModuleGroupComponent moduleGroup;
        }
    }
}

