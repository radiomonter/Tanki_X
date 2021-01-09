namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class NewsItemFilterEvent : Event
    {
        public bool Hide { get; set; }
    }
}

