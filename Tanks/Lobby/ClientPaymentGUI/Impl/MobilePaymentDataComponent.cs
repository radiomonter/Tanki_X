namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class MobilePaymentDataComponent : Component
    {
        public string PhoneNumber { get; set; }

        public string TransactionId { get; set; }
    }
}

