namespace Tanks.Battle.ClientHUD.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientGarage.API;

    public class RageHUDEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void PlayRageEffect(TriggerEffectExecuteEvent e, SingleNode<RageEffectComponent> effect, [JoinByTank] TankNode tank)
        {
            base.ScheduleEvent<PlayRageHUDEffectEvent>(tank);
        }

        [OnEventFire]
        public void PlayRageHUDEffect(PlayRageHUDEffectEvent e, TankNode tank, [JoinByUser, Combine] SlotCooldownStateNode slot, [JoinByModule] SingleNode<ItemButtonComponent> hud)
        {
            float cutTime = (((float) slot.inventoryCooldownState.CooldownTime) / 1000f) - (Date.Now.UnityTime - slot.inventoryCooldownState.CooldownStartTime.UnityTime);
            hud.component.CutCooldown(cutTime);
        }

        public class PlayRageHUDEffectEvent : Event
        {
        }

        [Not(typeof(EffectsFreeSlotComponent))]
        public class SlotCooldownStateNode : Node
        {
            public SlotUserItemInfoComponent slotUserItemInfo;
            public ModuleGroupComponent moduleGroup;
            public InventoryCooldownStateComponent inventoryCooldownState;
        }

        public class TankNode : Node
        {
            public TankComponent tank;
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankGroupComponent tankGroup;
        }
    }
}

