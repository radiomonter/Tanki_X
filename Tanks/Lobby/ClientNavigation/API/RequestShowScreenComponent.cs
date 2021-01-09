namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class RequestShowScreenComponent : Component
    {
        public ShowScreenEvent Event { get; set; }
    }
}

