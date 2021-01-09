namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ShowProfileScreenEvent : Event
    {
        public ShowProfileScreenEvent(long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}

