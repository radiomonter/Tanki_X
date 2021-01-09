namespace Tanks.Lobby.ClientFriends.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15cf3ece5a1L)]
    public class SortedFriendsIdsWithNicknamesLoaded : Event
    {
        public Dictionary<long, string> FriendsIdsAndNicknames { get; set; }
    }
}

