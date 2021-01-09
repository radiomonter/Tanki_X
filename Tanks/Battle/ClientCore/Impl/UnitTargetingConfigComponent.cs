namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4d28898047e8eL)]
    public class UnitTargetingConfigComponent : Component
    {
        public float TargetingPeriod { get; set; }

        public float WorkDistance { get; set; }
    }
}

