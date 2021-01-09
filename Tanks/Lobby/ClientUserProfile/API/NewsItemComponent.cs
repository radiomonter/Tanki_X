namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x158719aa476L)]
    public class NewsItemComponent : Component
    {
        public NewsItem Data { get; set; }

        [ProtocolTransient]
        public int ShowCount { get; set; }
    }
}

