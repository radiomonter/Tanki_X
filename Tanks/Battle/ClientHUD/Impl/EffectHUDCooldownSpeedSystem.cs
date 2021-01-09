namespace Tanks.Battle.ClientHUD.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientGarage.API;

    public class EffectHUDCooldownSpeedSystem : ECSSystem
    {
        [OnEventFire]
        public void ChangeCooldownSpeedHUDAnimation(BattleUserInventoryCooldownSpeedChangedEvent e, SingleNode<BattleUserInventoryCooldownSpeedComponent> battleUser, [JoinByUser, Combine] SlotCooldownStateNode slot, [JoinByModule] SingleNode<ItemButtonComponent> hud)
        {
            float time = (((float) slot.inventoryCooldownState.CooldownTime) / 1000f) - (Date.Now.UnityTime - slot.inventoryCooldownState.CooldownStartTime.UnityTime);
            hud.component.ChangeCooldown(time, battleUser.component.SpeedCoeff, slot.Entity.HasComponent<InventoryEnabledStateComponent>());
        }

        [OnEventFire]
        public void PlayChangeCooldownSpeedHUDAnimation(BattleUserInventoryCooldownSpeedChangedEvent e, SingleNode<BattleUserInventoryCooldownSpeedComponent> battleUser, [JoinByUser, Combine] HUDNodes.Modules.SlotWithModuleNode slot, [JoinByModule] SingleNode<ItemButtonComponent> hud)
        {
            hud.component.SetCooldownCoeff(battleUser.component.SpeedCoeff);
            hud.component.isRage = !hud.component.isRage;
        }

        [Not(typeof(EffectsFreeSlotComponent))]
        public class SlotCooldownStateNode : Node
        {
            public SlotUserItemInfoComponent slotUserItemInfo;
            public ModuleGroupComponent moduleGroup;
            public InventoryCooldownStateComponent inventoryCooldownState;
        }
    }
}

