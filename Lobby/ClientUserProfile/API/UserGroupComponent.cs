namespace Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [Shared, SerialVersionUID(0x676e8793084704f1L)]
    public class UserGroupComponent : GroupComponent
    {
        public UserGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public UserGroupComponent(long key) : base(key)
        {
        }
    }
}

