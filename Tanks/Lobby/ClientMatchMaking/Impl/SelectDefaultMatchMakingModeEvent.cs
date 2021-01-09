namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SelectDefaultMatchMakingModeEvent : Event
    {
        public Optional<Entity> DefaultMode { get; set; }
    }
}

