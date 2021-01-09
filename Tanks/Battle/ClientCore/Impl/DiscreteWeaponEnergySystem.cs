namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class DiscreteWeaponEnergySystem : ECSSystem
    {
        [OnEventFire]
        public void CheckInitialState(NodeAddedEvent evt, DiscreteWeaponEnergyNode weapon, [Context, JoinByTank] ActiveTankNode activeTank)
        {
            if (weapon.weaponEnergy.Energy == 1f)
            {
                weapon.weaponEnergyEsm.Esm.ChangeState<WeaponEnergyStates.WeaponEnergyFullState>();
            }
            else
            {
                weapon.weaponEnergyEsm.Esm.ChangeState<WeaponEnergyStates.WeaponEnergyReloadingState>();
            }
        }

        [OnEventFire]
        public void ReloadEnergy(TimeUpdateEvent evt, DiscreteWeaponEnergyReloadingNode weapon, [JoinByTank] ActiveTankNode activeTank)
        {
            float num = weapon.discreteWeaponEnergy.ReloadEnergyPerSec * evt.DeltaTime;
            weapon.weaponEnergy.Energy += num;
            if (weapon.weaponEnergy.Energy >= 1f)
            {
                weapon.weaponEnergy.Energy = 1f;
                weapon.weaponEnergyEsm.Esm.ChangeState<WeaponEnergyStates.WeaponEnergyFullState>();
            }
        }

        [OnEventFire]
        public void UnloadEnergyPerShot(BaseShotEvent evt, DiscreteWeaponEnergyNode weapon)
        {
            float unloadEnergyPerShot = weapon.discreteWeaponEnergy.UnloadEnergyPerShot;
            weapon.weaponEnergy.Energy -= unloadEnergyPerShot;
            weapon.weaponEnergy.Energy = Mathf.Clamp(weapon.weaponEnergy.Energy, 0f, 1f);
            weapon.weaponEnergyEsm.Esm.ChangeState<WeaponEnergyStates.WeaponEnergyReloadingState>();
        }

        public class ActiveTankNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankActiveStateComponent tankActiveState;
        }

        public class DiscreteWeaponEnergyNode : Node
        {
            public DiscreteWeaponEnergyComponent discreteWeaponEnergy;
            public WeaponEnergyComponent weaponEnergy;
            public DiscreteWeaponComponent discreteWeapon;
            public WeaponEnergyESMComponent weaponEnergyEsm;
            public TankGroupComponent tankGroup;
        }

        public class DiscreteWeaponEnergyReloadingNode : Node
        {
            public DiscreteWeaponEnergyComponent discreteWeaponEnergy;
            public WeaponEnergyComponent weaponEnergy;
            public WeaponEnergyESMComponent weaponEnergyEsm;
            public DiscreteWeaponComponent discreteWeapon;
            public WeaponEnergyReloadingStateComponent weaponEnergyReloadingState;
            public TankGroupComponent tankGroup;
        }
    }
}

