namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ItemPacksComponent : Component
    {
        public List<int> ForXPrice { get; set; }

        public List<int> ForPrice { get; set; }
    }
}

