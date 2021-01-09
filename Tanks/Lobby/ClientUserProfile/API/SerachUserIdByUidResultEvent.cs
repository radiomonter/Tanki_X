namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15626dfd25aL)]
    public class SerachUserIdByUidResultEvent : Event
    {
        public bool Found { get; set; }

        public long UserId { get; set; }
    }
}

