namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15e36ef8f02L)]
    public class ChangeScreenEvent : Event
    {
        public ChangeScreenEvent(string currentScreen, string nextScreen, double duration)
        {
            this.CurrentScreen = currentScreen;
            this.NextScreen = nextScreen;
            this.Duration = duration;
        }

        public string CurrentScreen { get; set; }

        public string NextScreen { get; set; }

        public double Duration { get; set; }
    }
}

