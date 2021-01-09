namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CurrentSeasonNameComponent : Component
    {
        public string SeasonName { get; set; }
    }
}

