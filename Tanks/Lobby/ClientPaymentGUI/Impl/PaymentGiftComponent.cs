namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class PaymentGiftComponent : Component
    {
        public PaymentGiftComponent(long gift)
        {
            this.Gift = gift;
        }

        public long Gift { get; private set; }
    }
}

