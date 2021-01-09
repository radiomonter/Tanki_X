namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x16384a833f8L)]
    public class PackPriceChangedEvent : Event
    {
        public Dictionary<int, int> PackPrice { get; set; }

        public Dictionary<int, int> PackXPrice { get; set; }

        public Dictionary<int, int> OldPackPrice { get; set; }

        public Dictionary<int, int> OldPackXPrice { get; set; }
    }
}

