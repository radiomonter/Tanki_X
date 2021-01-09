namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class HangarCameraFlightState
    {
        public class ArcFlightState : Node
        {
            public HangarCameraArcFlightComponent hangarCameraArcFlight;
        }

        public class EmptyState : Node
        {
        }

        public class LinearFlightState : Node
        {
            public HangarCameraLinearFlightComponent hangarCameraLinearFlight;
        }
    }
}

