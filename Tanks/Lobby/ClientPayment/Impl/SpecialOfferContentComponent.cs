namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SpecialOfferContentComponent : Component
    {
        public int SalePercent { get; set; }

        public bool HighlightTitle { get; set; }

        public bool ShowItemsList { get; set; }
    }
}

