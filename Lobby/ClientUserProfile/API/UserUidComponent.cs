namespace Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-5477085396086342998L)]
    public class UserUidComponent : Component
    {
        public UserUidComponent()
        {
        }

        public UserUidComponent(string uid)
        {
            this.Uid = uid;
        }

        public string Uid { get; set; }
    }
}

