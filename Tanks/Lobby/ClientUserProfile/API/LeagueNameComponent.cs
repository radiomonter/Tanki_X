namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class LeagueNameComponent : Component
    {
        public string Name { get; set; }

        public string NameAccusative { get; set; }
    }
}

