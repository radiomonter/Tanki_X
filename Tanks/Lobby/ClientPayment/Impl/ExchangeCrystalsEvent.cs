namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x156ff3fcb77L)]
    public class ExchangeCrystalsEvent : Event
    {
        public long XCrystals { get; set; }
    }
}

