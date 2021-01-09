namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x2bfb0f25329ee65dL)]
    public class SplashWeaponComponent : Component
    {
        public float RadiusOfMinSplashDamage { get; set; }

        public float RadiusOfMaxSplashDamage { get; set; }

        public float MinSplashDamagePercent { get; set; }
    }
}

