namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-615965945505672897L)]
    public class TankMovementComponent : Component
    {
        public Tanks.Battle.ClientCore.Impl.Movement Movement { get; set; }

        public Tanks.Battle.ClientCore.Impl.MoveControl MoveControl { get; set; }

        public float WeaponRotation { get; set; }

        public float WeaponControl { get; set; }
    }
}

