namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class LocalHammerMagazineSystem : ECSSystem
    {
        [OnEventFire]
        public void InitLocalMagazineStorage(NodeAddedEvent evt, InitialHammerMagazineNode weapon)
        {
            int currentCartridgeCount = weapon.magazineStorage.CurrentCartridgeCount;
            weapon.Entity.AddComponent(new MagazineLocalStorageComponent(currentCartridgeCount));
        }

        [OnEventFire]
        public void ResetLocalMagazineStorage(ExecuteEnergyInjectionEvent e, LocalHammerMagazineNode weapon)
        {
            weapon.magazineLocalStorage.CurrentCartridgeCount = weapon.magazineWeapon.MaxCartridgeCount;
        }

        [OnEventFire]
        public void ResetLocalMagazineStorage(NodeAddedEvent evt, LocalHammerMagazineNode weapon, [JoinByTank, Context] TankIncarnationNode tank)
        {
            weapon.magazineLocalStorage.CurrentCartridgeCount = weapon.magazineWeapon.MaxCartridgeCount;
        }

        [OnEventComplete]
        public void UpdateLocalMagazineStorage(BaseShotEvent evt, LocalHammerMagazineNode weapon)
        {
            int maxCartridgeCount = weapon.magazineWeapon.MaxCartridgeCount;
            weapon.magazineLocalStorage.CurrentCartridgeCount = (weapon.magazineLocalStorage.CurrentCartridgeCount != 1) ? (weapon.magazineLocalStorage.CurrentCartridgeCount - 1) : maxCartridgeCount;
        }

        public class InitialHammerMagazineNode : Node
        {
            public MagazineStorageComponent magazineStorage;
            public MagazineWeaponComponent magazineWeapon;
            public HammerComponent hammer;
        }

        public class LocalHammerMagazineNode : Node
        {
            public MagazineLocalStorageComponent magazineLocalStorage;
            public MagazineWeaponComponent magazineWeapon;
            public TankGroupComponent tankGroup;
        }

        public class TankIncarnationNode : Node
        {
            public TankIncarnationComponent tankIncarnation;
            public TankGroupComponent tankGroup;
        }
    }
}

