namespace Tanks.Lobby.ClientPayment.Impl
{
    using Lobby.ClientPayment.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [Shared, SerialVersionUID(0x8d47a66c6ce2696L)]
    public class SteamBuyGoodsEvent : ProcessPaymentEvent
    {
    }
}

