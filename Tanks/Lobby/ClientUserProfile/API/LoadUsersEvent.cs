namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15398aad905L)]
    public class LoadUsersEvent : Event
    {
        public LoadUsersEvent(long requestEntityId, HashSet<long> usersId)
        {
            this.RequestEntityId = requestEntityId;
            this.UsersId = usersId;
        }

        public LoadUsersEvent(long requestEntityId, params long[] usersIds)
        {
            this.RequestEntityId = requestEntityId;
            this.UsersId = new HashSet<long>(usersIds);
        }

        public long RequestEntityId { get; set; }

        public HashSet<long> UsersId { get; set; }
    }
}

