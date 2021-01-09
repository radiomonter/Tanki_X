namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class BonusConfigComponent : Component
    {
        public float FallSpeed { get; set; }

        public float AngularSpeed { get; set; }

        public float SwingFreq { get; set; }

        public float SwingAngle { get; set; }

        public float AlignmentToGroundAngularSpeed { get; set; }

        public float AppearingOnGroundTime { get; set; }

        public float SpawnDuration { get; set; }
    }
}

