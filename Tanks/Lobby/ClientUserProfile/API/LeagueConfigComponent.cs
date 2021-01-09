namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15de0ad9c05L)]
    public class LeagueConfigComponent : Component
    {
        public int LeagueIndex { get; set; }

        public double ReputationToEnter { get; set; }
    }
}

