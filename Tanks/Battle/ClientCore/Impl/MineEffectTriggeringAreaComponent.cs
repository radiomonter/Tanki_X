namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4dd982d3719d3L)]
    public class MineEffectTriggeringAreaComponent : Component
    {
        public float Radius { get; set; }
    }
}

