namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15e18ccc612L)]
    public class CurrentSeasonRewardForClientComponent : Component
    {
        public List<EndSeasonRewardItem> Rewards { get; set; }
    }
}

