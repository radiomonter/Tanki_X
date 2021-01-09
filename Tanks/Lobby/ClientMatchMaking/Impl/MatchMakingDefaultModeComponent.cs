namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15d11d73c41L)]
    public class MatchMakingDefaultModeComponent : Component
    {
        public int MinimalBattles { get; set; }
    }
}

