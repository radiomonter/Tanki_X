namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ProfileScreenContextComponent : Component
    {
        public ProfileScreenContextComponent()
        {
        }

        public ProfileScreenContextComponent(long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}

