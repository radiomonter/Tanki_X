namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d3b0b0e7a4f3f3L)]
    public class DefaultSkinComponent : Component
    {
        public long DefaultSkin { get; set; }
    }
}

