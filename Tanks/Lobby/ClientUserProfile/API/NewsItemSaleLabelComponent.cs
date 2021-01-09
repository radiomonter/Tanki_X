namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x158e3c9e00cL)]
    public class NewsItemSaleLabelComponent : Component
    {
        public string Text { get; set; }
    }
}

