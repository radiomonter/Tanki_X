namespace Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15281c812c9L)]
    public class PaymentIsCancelledEvent : Event
    {
        public int ErrorCode { get; set; }
    }
}

