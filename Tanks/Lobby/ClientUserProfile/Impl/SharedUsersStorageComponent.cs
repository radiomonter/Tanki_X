namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SharedUsersStorageComponent : Component
    {
        public SharedUsersStorageComponent()
        {
            this.UserId2EntityIdMap = new HashMultiMap<long, long>();
        }

        public HashMultiMap<long, long> UserId2EntityIdMap { get; private set; }
    }
}

