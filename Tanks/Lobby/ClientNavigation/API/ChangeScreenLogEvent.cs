namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ChangeScreenLogEvent : Event
    {
        public ChangeScreenLogEvent(LogScreen nextScreen)
        {
            this.NextScreen = nextScreen;
        }

        public LogScreen NextScreen { get; set; }
    }
}

