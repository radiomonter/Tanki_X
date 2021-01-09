namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15373b83e86L)]
    public class RequestChangeUserEmailEvent : Event
    {
        public RequestChangeUserEmailEvent()
        {
        }

        public RequestChangeUserEmailEvent(string email)
        {
            this.Email = email;
        }

        public string Email { get; set; }
    }
}

