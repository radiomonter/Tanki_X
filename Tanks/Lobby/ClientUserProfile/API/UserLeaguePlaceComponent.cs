namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class UserLeaguePlaceComponent : Component
    {
        public long Place { get; set; }
    }
}

