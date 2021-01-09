namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientGarage.API;

    public class ItemButtonCooldownStateSystem : ECSSystem
    {
        [OnEventFire]
        public void InitializeCooldownFill(NodeAddedEvent e, SlotCooldownStateNode slot, [Context, JoinByModule] ItemButtonNode itemButton)
        {
            this.StartCooldown(slot, itemButton);
        }

        [OnEventFire]
        public void InitializeCooldownFill(NodeRemoveEvent e, SlotCooldownStateNode slot, [JoinByModule] ItemButtonNode itemButton)
        {
            itemButton.itemButton.FinishCooldown();
            if (slot.Entity.HasComponent<InventoryEnabledStateComponent>())
            {
                itemButton.itemButton.Enable();
            }
            else
            {
                itemButton.itemButton.Disable();
            }
        }

        private void StartCooldown(SlotCooldownStateNode slot, ItemButtonNode itemButton)
        {
            float timeInSec = (((float) slot.inventoryCooldownState.CooldownTime) / 1000f) - (Date.Now.UnityTime - slot.inventoryCooldownState.CooldownStartTime.UnityTime);
            if (itemButton.itemButton.isRage)
            {
                itemButton.itemButton.StartRageCooldown(timeInSec, slot.Entity.HasComponent<InventoryEnabledStateComponent>());
            }
            else
            {
                itemButton.itemButton.StartCooldown(timeInSec, slot.Entity.HasComponent<InventoryEnabledStateComponent>());
                if (itemButton.itemButton.ammunitionCountWasIncreased && ((itemButton.itemButton.MaxItemAmmunitionCount > 1) && (itemButton.itemButton.ItemAmmunitionCount == 1)))
                {
                    itemButton.itemButton.FinishCooldown();
                }
            }
        }

        public class ItemButtonNode : Node
        {
            public ItemButtonComponent itemButton;
            public ModuleGroupComponent moduleGroup;
        }

        public class SlotCooldownStateNode : Node
        {
            public SlotUserItemInfoComponent slotUserItemInfo;
            public ModuleGroupComponent moduleGroup;
            public InventoryCooldownStateComponent inventoryCooldownState;
        }
    }
}

