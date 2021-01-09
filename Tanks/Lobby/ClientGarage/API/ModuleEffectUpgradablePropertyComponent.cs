namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public abstract class ModuleEffectUpgradablePropertyComponent : Component
    {
        protected ModuleEffectUpgradablePropertyComponent()
        {
        }

        public bool LinearInterpolation { get; set; }

        public float[] UpgradeLevel2Values { get; set; }

        public Dictionary<long, EffectProperty> EquipmentProperties { get; set; }
    }
}

