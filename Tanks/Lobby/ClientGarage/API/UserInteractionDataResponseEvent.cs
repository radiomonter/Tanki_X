namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x148d74663fdL)]
    public class UserInteractionDataResponseEvent : Event
    {
        public long UserId { get; set; }

        public string UserUid { get; set; }

        public bool CanRequestFrendship { get; set; }

        public bool FriendshipRequestWasSend { get; set; }

        public bool Muted { get; set; }

        public bool Reported { get; set; }
    }
}

