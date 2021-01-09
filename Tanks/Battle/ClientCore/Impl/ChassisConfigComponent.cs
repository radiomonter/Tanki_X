﻿namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ChassisConfigComponent : Component
    {
        public bool ReverseBackTurn { get; set; }

        public float TrackSeparation { get; set; }

        public float SuspensionRayOffsetY { get; set; }

        public int NumRaysPerTrack { get; set; }

        public float MaxRayLength { get; set; }

        public float NominalRayLength { get; set; }
    }
}

