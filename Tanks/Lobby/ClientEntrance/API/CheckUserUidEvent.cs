namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14eceebd8ceL)]
    public class CheckUserUidEvent : Event
    {
        public CheckUserUidEvent()
        {
        }

        public CheckUserUidEvent(string uid)
        {
            this.Uid = uid;
        }

        public string Uid { get; set; }
    }
}

