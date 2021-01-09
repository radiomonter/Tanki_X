namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15c8239eb79L)]
    public class MatchMakingLobbyStartTimeComponent : Component
    {
        public Date StartTime { get; set; }
    }
}

