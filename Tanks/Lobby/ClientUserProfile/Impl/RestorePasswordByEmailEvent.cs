namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x153f520139aL)]
    public class RestorePasswordByEmailEvent : Event
    {
        public string Email { get; set; }
    }
}

