namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;

    public class ShaftEnergyBarSystem : ECSSystem
    {
        [OnEventFire]
        public void Init(NodeAddedEvent e, ShaftEnergyNode weapon, [JoinByTank, Context] HUDNodes.SelfTankNode tank, SingleNode<MainHUDComponent> hud)
        {
            hud.component.EnergyBarEnabled = true;
            hud.component.MaxEnergyValue = 1f;
            hud.component.CurrentEnergyValue = 0f;
            hud.component.EnergyAmountPerSegment = 1f;
        }

        [OnEventFire]
        public void UpdateEnergy(TimeUpdateEvent e, ShaftEnergyNode weapon, [JoinByTank] HUDNodes.ActiveSelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud)
        {
            CooldownTimerComponent cooldownTimer = weapon.cooldownTimer;
            if ((weapon.weaponEnergy.Energy >= weapon.shaftEnergy.UnloadEnergyPerQuickShot) && ((cooldownTimer.CooldownTimerSec <= 0f) && weapon.Entity.HasComponent<ShootableComponent>()))
            {
                if (!weapon.Entity.HasComponent<BlinkMarkerComponent>())
                {
                    weapon.Entity.AddComponent<BlinkMarkerComponent>();
                    hud.component.EnergyBlink(true);
                }
            }
            else
            {
                if (weapon.Entity.HasComponent<BlinkMarkerComponent>())
                {
                    weapon.Entity.RemoveComponent<BlinkMarkerComponent>();
                }
                if (InputManager.GetActionKeyDown(ShotActions.SHOT))
                {
                    hud.component.EnergyBlink(false);
                }
            }
            hud.component.CurrentEnergyValue = weapon.weaponEnergy.Energy;
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class BlinkMarkerComponent : Component
        {
        }

        public class ShaftEnergyNode : Node
        {
            public ShaftEnergyComponent shaftEnergy;
            public WeaponEnergyComponent weaponEnergy;
            public TankGroupComponent tankGroup;
            public CooldownTimerComponent cooldownTimer;
        }

        public class ShaftReadyEnergyNode : ShaftEnergyBarSystem.ShaftEnergyNode
        {
            public ShaftIdleStateComponent shaftIdleState;
        }

        public class ShaftReloadingEnergyNode : WeaponEnergyStates.WeaponEnergyReloadingState
        {
            public ShaftEnergyComponent shaftEnergy;
            public WeaponEnergyComponent weaponEnergy;
        }
    }
}

