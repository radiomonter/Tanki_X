namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15ab46f648eL)]
    public class CalculateCompensationResponseEvent : Event
    {
        public long Amount { get; set; }
    }
}

