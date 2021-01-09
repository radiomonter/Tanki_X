namespace Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x152a74123b8L), Shared]
    public class AdyenBuyGoodsByCardEvent : ProcessPaymentEvent
    {
        public string EncrypedCard { get; set; }
    }
}

