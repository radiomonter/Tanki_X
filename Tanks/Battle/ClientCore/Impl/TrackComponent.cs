namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class TrackComponent : Component
    {
        public Track LeftTrack { get; set; }

        public Track RightTrack { get; set; }
    }
}

