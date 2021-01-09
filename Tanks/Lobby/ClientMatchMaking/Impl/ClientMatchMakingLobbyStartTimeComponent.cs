namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ClientMatchMakingLobbyStartTimeComponent : Component
    {
        public Date StartTime { get; set; }
    }
}

