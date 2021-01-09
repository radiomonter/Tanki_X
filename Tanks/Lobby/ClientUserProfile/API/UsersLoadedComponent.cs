namespace Tanks.Lobby.ClientUserProfile.API
{
    using System;
    using System.Collections.Generic;

    public class UsersLoadedComponent : LoadUsersComponent
    {
        public UsersLoadedComponent(HashSet<long> userIds) : base(userIds)
        {
        }
    }
}

