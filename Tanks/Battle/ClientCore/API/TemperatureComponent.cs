namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x5c9dadbdfaa77c9cL)]
    public class TemperatureComponent : Component
    {
        public float Temperature { get; set; }
    }
}

