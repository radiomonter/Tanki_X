namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x151ec4e88e9L)]
    public class RequestLoadUserProfileEvent : Event
    {
        public RequestLoadUserProfileEvent()
        {
        }

        public RequestLoadUserProfileEvent(long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}

