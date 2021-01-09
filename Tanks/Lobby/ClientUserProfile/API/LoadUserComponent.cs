namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class LoadUserComponent : Component
    {
        public LoadUserComponent(long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; private set; }
    }
}

