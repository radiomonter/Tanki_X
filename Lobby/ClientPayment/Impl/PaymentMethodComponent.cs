namespace Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15281a805aaL)]
    public class PaymentMethodComponent : Component
    {
        public string ProviderName { get; set; }

        public string MethodName { get; set; }

        public string ShownName { get; set; }

        public bool IsTerminal { get; set; }
    }
}

