namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d3c815fc65cfe1L)]
    public class ChangeUsernameEvent : Event
    {
        public ChangeUsernameEvent()
        {
        }

        public ChangeUsernameEvent(string uid)
        {
            this.Uid = uid;
        }

        public string Uid { get; set; }
    }
}

