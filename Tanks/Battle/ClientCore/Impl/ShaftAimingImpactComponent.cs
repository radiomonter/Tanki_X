namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14ece82326fL)]
    public class ShaftAimingImpactComponent : Component
    {
        public float MaxImpactForce { get; set; }
    }
}

