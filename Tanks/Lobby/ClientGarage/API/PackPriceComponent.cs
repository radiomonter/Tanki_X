namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class PackPriceComponent : Component
    {
        public Dictionary<int, int> PackPrice { get; set; }

        public Dictionary<int, int> PackXPrice { get; set; }
    }
}

