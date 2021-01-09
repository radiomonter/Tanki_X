namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [SerialVersionUID(0x15a2866a8dfL), Shared]
    public class NotificationGroupComponent : GroupComponent
    {
        public NotificationGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public NotificationGroupComponent(long key) : base(key)
        {
        }
    }
}

