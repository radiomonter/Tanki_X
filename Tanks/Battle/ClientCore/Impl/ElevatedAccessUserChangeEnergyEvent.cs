namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15fe84abbadL)]
    public class ElevatedAccessUserChangeEnergyEvent : Event
    {
        public int Count { get; set; }
    }
}

