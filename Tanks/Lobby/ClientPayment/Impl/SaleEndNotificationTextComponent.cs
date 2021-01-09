namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SaleEndNotificationTextComponent : Component
    {
        public string Message { get; set; }
    }
}

