namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x15e03f204ecL)]
    public class LeagueEnergyConfigComponent : Component
    {
        public long Cost { get; set; }

        public long Capacity { get; set; }
    }
}

