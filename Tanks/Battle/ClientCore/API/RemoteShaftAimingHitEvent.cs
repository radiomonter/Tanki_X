namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x41d419766133f2dcL)]
    public class RemoteShaftAimingHitEvent : RemoteHitEvent
    {
        public float HitPower { get; set; }
    }
}

