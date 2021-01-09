namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class LoadUsersComponent : Component
    {
        public LoadUsersComponent(HashSet<long> userIds)
        {
            this.UserIds = userIds;
        }

        public HashSet<long> UserIds { get; private set; }
    }
}

