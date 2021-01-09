namespace Tanks.Lobby.ClientNavigation.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x152813bef6bL)]
    public class EnterScreenEvent : Event
    {
        public EnterScreenEvent()
        {
        }

        public EnterScreenEvent(string screen)
        {
            this.Screen = screen;
        }

        public string Screen { get; set; }
    }
}

