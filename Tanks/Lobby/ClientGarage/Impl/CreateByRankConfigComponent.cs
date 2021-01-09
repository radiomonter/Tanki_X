namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x15c1a5ee741L)]
    public class CreateByRankConfigComponent : Component
    {
        public int[] UserRankListToCreateItem { get; set; }
    }
}

