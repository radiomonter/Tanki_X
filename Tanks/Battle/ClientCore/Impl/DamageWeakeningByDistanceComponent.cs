namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x27d25aaaf82cf8f1L)]
    public class DamageWeakeningByDistanceComponent : Component
    {
        public float RadiusOfMaxDamage { get; set; }

        public float RadiusOfMinDamage { get; set; }

        public float MinDamagePercent { get; set; }
    }
}

