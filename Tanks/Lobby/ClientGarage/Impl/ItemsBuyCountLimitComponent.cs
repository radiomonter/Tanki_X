namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x15c445d0eebL)]
    public class ItemsBuyCountLimitComponent : Component
    {
        public int Limit { get; set; }
    }
}

