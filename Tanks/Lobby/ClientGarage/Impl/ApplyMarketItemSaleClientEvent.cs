namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x16382a96cfbL)]
    public class ApplyMarketItemSaleClientEvent : Event
    {
        public Date EndDate { get; set; }
    }
}

