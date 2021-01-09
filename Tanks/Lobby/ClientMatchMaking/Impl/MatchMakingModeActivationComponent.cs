namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class MatchMakingModeActivationComponent : Component
    {
        public bool Active { get; set; }
    }
}

