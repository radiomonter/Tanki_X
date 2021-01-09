namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x152ae4cbeedL)]
    public class UserInteractionDataRequestEvent : Event
    {
        public long UserId { get; set; }
    }
}

