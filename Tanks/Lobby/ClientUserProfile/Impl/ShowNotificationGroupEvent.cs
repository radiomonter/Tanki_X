namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15a41ad3d5eL)]
    public class ShowNotificationGroupEvent : Event
    {
        public int ExpectedMembersCount { get; set; }
    }
}

