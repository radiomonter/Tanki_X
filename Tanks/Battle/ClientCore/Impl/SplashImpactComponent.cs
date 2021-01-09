namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14efd8ef6e3L)]
    public class SplashImpactComponent : Component
    {
        public float ImpactForce { get; set; }
    }
}

