namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x47b1ac070015903aL)]
    public class TankSemiActiveStateComponent : Component
    {
        public int ActivationTime { get; set; }
    }
}

