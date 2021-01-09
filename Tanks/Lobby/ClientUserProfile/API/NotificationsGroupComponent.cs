namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class NotificationsGroupComponent : GroupComponent
    {
        public NotificationsGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public NotificationsGroupComponent(long key) : base(key)
        {
        }
    }
}

