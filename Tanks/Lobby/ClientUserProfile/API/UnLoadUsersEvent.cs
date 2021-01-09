namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15398abce18L)]
    public class UnLoadUsersEvent : Event
    {
        public UnLoadUsersEvent(HashSet<long> userIds)
        {
            this.UserIds = userIds;
        }

        public UnLoadUsersEvent(params long[] userIds)
        {
            this.UserIds = new HashSet<long>(userIds);
        }

        public HashSet<long> UserIds { get; set; }
    }
}

