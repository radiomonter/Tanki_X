namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1562698e0f6L)]
    public class SearchUserIdByUidEvent : Event
    {
        public string Uid { get; set; }
    }
}

