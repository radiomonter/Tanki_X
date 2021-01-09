namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;

    public class DiscreteEnergyBarSystem : ECSSystem
    {
        [OnEventFire]
        public void Energy(TimeUpdateEvent e, DiscreteEnergyNode weapon, [JoinByTank] HUDNodes.ActiveSelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud)
        {
            if (InputManager.GetActionKeyDown(ShotActions.SHOT) && !weapon.Entity.HasComponent<ShootableComponent>())
            {
                hud.component.EnergyBlink(false);
            }
            else
            {
                if ((weapon.weaponEnergy.Energy < weapon.discreteWeaponEnergy.UnloadEnergyPerShot) && InputManager.GetActionKeyDown(ShotActions.SHOT))
                {
                    hud.component.EnergyBlink(false);
                }
                float currentEnergyValue = hud.component.CurrentEnergyValue;
                hud.component.CurrentEnergyValue = weapon.weaponEnergy.Energy;
                if ((hud.component.CurrentEnergyValue >= 1f) && (currentEnergyValue < 1f))
                {
                    hud.component.EnergyBlink(true);
                }
            }
        }

        [OnEventFire]
        public void Init(NodeAddedEvent e, DiscreteEnergyNode weapon, [JoinByTank, Context] HUDNodes.SelfTankNode tank, SingleNode<MainHUDComponent> hud)
        {
            hud.component.EnergyBarEnabled = true;
            hud.component.MaxEnergyValue = 1f;
            hud.component.CurrentEnergyValue = 0f;
            hud.component.EnergyAmountPerSegment = 1f;
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        [Not(typeof(TwinsComponent))]
        public class DiscreteEnergyNode : HUDNodes.BaseWeaponNode
        {
            public DiscreteWeaponEnergyComponent discreteWeaponEnergy;
        }
    }
}

