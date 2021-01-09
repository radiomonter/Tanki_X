namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientProfile.API;

    public class GetUserLevelInfoEvent : Event
    {
        public LevelInfo Info { get; set; }
    }
}

