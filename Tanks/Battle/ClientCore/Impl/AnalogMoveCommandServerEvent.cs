namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x240944cb160aea67L)]
    public class AnalogMoveCommandServerEvent : Event
    {
        [ProtocolOptional]
        public Tanks.Battle.ClientCore.Impl.Movement? Movement { get; set; }

        [ProtocolOptional]
        public Tanks.Battle.ClientCore.Impl.MoveControl? MoveControl { get; set; }

        [ProtocolOptional]
        public float? WeaponRotation { get; set; }

        [ProtocolOptional]
        public float? WeaponControl { get; set; }
    }
}

