namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x724de522488169adL)]
    public class WeaponEnergyComponent : Component
    {
        public WeaponEnergyComponent()
        {
        }

        public WeaponEnergyComponent(float energy)
        {
            this.Energy = energy;
        }

        public float Energy { get; set; }
    }
}

