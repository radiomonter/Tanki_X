namespace Tanks.Lobby.ClientFriends.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15374b53eeaL)]
    public class SortedFriendsIdsLoadedEvent : Event
    {
        public Dictionary<long, string> friendsAcceptedIds { get; set; }

        public Dictionary<long, string> friendsIncomingIds { get; set; }

        public Dictionary<long, string> friendsOutgoingIds { get; set; }
    }
}

