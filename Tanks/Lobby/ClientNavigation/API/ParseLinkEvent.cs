namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ParseLinkEvent : Event
    {
        public string Link { get; set; }

        public EventBuilder CustomNavigationEvent { get; set; }

        public Type ScreenType { get; set; }

        public Entity ScreenContext { get; set; }

        public bool ScreenContextAutoDelete { get; set; }

        public string ParseMessage { get; set; }
    }
}

