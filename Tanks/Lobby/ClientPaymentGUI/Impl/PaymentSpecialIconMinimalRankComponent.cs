namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;

    public class PaymentSpecialIconMinimalRankComponent : LocalizedControl, Component
    {
        private string minimalRank;

        public string MinimalRank
        {
            get => 
                this.minimalRank;
            set => 
                this.minimalRank = value;
        }
    }
}

