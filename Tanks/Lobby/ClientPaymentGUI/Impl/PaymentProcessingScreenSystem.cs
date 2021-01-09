namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class PaymentProcessingScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, ScreenNode screen)
        {
            screen.Entity.AddComponent<LockedScreenComponent>();
        }

        public class ScreenNode : Node
        {
            public PaymentProcessingScreenComponent paymentProcessingScreen;
        }
    }
}

