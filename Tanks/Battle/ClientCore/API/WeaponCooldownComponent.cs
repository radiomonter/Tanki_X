namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x62be3f0d01ca900bL)]
    public class WeaponCooldownComponent : Component
    {
        public WeaponCooldownComponent()
        {
        }

        public WeaponCooldownComponent(float cooldownIntervalSec)
        {
            this.CooldownIntervalSec = cooldownIntervalSec;
        }

        public float CooldownIntervalSec { get; set; }
    }
}

