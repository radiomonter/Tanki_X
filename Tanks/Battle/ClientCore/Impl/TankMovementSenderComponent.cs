namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class TankMovementSenderComponent : Component
    {
        public double LastSentMovementTime { get; set; }

        public double LastSentWeaponRotationTime { get; set; }

        public Movement? LastSentMovement { get; set; }

        public bool LastHasCollision { get; set; }

        public Movement LastPhysicsMovement { get; set; }
    }
}

