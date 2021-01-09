namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class WeaponEnergySystem : ECSSystem
    {
        [OnEventFire]
        public void DestroyWeaponEnergyStates(NodeRemoveEvent evt, ActiveTankNode node, [JoinByTank] WeaponEnergyWithESMNode weapon)
        {
            weapon.Entity.RemoveComponent<WeaponEnergyESMComponent>();
        }

        [OnEventFire]
        public void InitWeaponEnergyStates(NodeAddedEvent evt, WeaponEnergyNode weapon, [Context, JoinByTank] ActiveTankNode activeTank)
        {
            WeaponEnergyESMComponent component = new WeaponEnergyESMComponent();
            EntityStateMachine esm = component.Esm;
            weapon.weaponEnergy.Energy = 1f;
            esm.AddState<WeaponEnergyStates.WeaponEnergyFullState>();
            esm.AddState<WeaponEnergyStates.WeaponEnergyReloadingState>();
            esm.AddState<WeaponEnergyStates.WeaponEnergyUnloadingState>();
            weapon.Entity.AddComponent(component);
        }

        public class ActiveTankNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankActiveStateComponent tankActiveState;
        }

        public class WeaponEnergyNode : Node
        {
            public TankGroupComponent tankGroup;
            public WeaponEnergyComponent weaponEnergy;
        }

        public class WeaponEnergyWithESMNode : Node
        {
            public TankGroupComponent tankGroup;
            public WeaponEnergyComponent weaponEnergy;
        }
    }
}

