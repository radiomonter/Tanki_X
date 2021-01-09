namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class NavigateLinkEvent : Event
    {
        public string Link { get; set; }
    }
}

