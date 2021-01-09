namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class HangarLocationsComponent : Component
    {
        public Dictionary<HangarLocation, Transform> Locations { get; set; }
    }
}

