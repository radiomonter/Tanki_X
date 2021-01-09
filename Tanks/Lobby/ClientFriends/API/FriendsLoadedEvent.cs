namespace Tanks.Lobby.ClientFriends.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x151dd8897d3L)]
    public class FriendsLoadedEvent : Event
    {
        public HashSet<long> AcceptedFriendsIds { get; set; }

        public HashSet<long> IncommingFriendsIds { get; set; }

        public HashSet<long> OutgoingFriendsIds { get; set; }
    }
}

