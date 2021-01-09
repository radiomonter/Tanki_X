namespace Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x167e966c483L)]
    public class UserAvatarComponent : Component
    {
        public UserAvatarComponent()
        {
        }

        public UserAvatarComponent(string id)
        {
            this.Id = id;
        }

        public string Id { get; set; }
    }
}

