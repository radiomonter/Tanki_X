namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ChassisTrackControllerComponent : Component
    {
        public TrackController LeftTrack { get; set; }

        public TrackController RightTrack { get; set; }
    }
}

