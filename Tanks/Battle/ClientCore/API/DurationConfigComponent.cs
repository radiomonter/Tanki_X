namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x6b17458a140c03cL)]
    public class DurationConfigComponent : Component
    {
        public long Duration { get; set; }
    }
}

