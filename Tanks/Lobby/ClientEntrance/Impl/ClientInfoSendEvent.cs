namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x154f203a0f4L)]
    public class ClientInfoSendEvent : Event
    {
        public ClientInfoSendEvent(string settings)
        {
            this.Settings = settings;
        }

        public string Settings { get; set; }
    }
}

