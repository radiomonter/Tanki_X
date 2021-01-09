namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-9112902007903352542L)]
    public class AnalogMoveCommandEvent : Event
    {
        public void Init(ref Tanks.Battle.ClientCore.Impl.Movement? movement, Tanks.Battle.ClientCore.Impl.MoveControl? moveControl, float? weaponRotation, float? weaponControl)
        {
            this.Movement = movement;
            this.MoveControl = moveControl;
            this.WeaponRotation = weaponRotation;
            this.WeaponControl = weaponControl;
        }

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

