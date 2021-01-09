namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class HangarCameraViewState
    {
        public class FlightToLocationState : Node
        {
            public HangarCameraFlightToLocationComponent hangarCameraFlightToLocation;
        }

        public class FlightToTankState : Node
        {
            public HangarCameraFlightToTankComponent hangarCameraFlightToTank;
        }

        public class LocationViewState : Node
        {
            public HangarCameraLocationViewComponent hangarCameraLocationView;
        }

        public class TankViewState : Node
        {
            public HangarCameraTankViewComponent hangarCameraTankView;
        }
    }
}

