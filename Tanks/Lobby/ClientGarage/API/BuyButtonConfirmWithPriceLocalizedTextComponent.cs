namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class BuyButtonConfirmWithPriceLocalizedTextComponent : Component
    {
        public string BuyText { get; set; }

        public string ForText { get; set; }

        public string ConfirmText { get; set; }

        public string CancelText { get; set; }
    }
}

