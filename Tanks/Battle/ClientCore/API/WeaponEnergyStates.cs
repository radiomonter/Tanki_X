namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class WeaponEnergyStates
    {
        public class WeaponEnergyFullState : Node
        {
            public WeaponEnergyFullStateComponent weaponEnergyFullState;
        }

        public class WeaponEnergyReloadingState : Node
        {
            public WeaponEnergyReloadingStateComponent weaponEnergyReloadingState;
        }

        public class WeaponEnergyUnloadingState : Node
        {
            public WeaponEnergyUnloadingStateComponent weaponEnergyUnloadingState;
        }
    }
}

