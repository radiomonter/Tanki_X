namespace Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ProcessPaymentEvent : Event
    {
        public long TotalAmount { get; set; }
    }
}

