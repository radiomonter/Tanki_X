namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1584dd31fbeL)]
    public class ClientLaunchEvent : Event
    {
        public ClientLaunchEvent(string webId)
        {
            this.WebId = webId;
        }

        public string WebId { get; set; }
    }
}

