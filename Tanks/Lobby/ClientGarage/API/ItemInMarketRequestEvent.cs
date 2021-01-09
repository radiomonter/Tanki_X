namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System.Collections.Generic;

    public class ItemInMarketRequestEvent : Event
    {
        public readonly Dictionary<long, string> marketItems = new Dictionary<long, string>();
    }
}

