namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientNavigation.API;

    public class GoodsSelectionScreenComponent : LocalizedScreenComponent, PaymentScreen
    {
        private Tanks.Lobby.ClientPaymentGUI.Impl.XCrystalsDataProvider xCrystalsDataProvider;
        private Tanks.Lobby.ClientPaymentGUI.Impl.SpecialOfferDataProvider specialOfferDataProvider;

        protected override void Awake()
        {
            base.Awake();
            this.xCrystalsDataProvider = base.GetComponentInChildren<Tanks.Lobby.ClientPaymentGUI.Impl.XCrystalsDataProvider>();
            this.specialOfferDataProvider = base.GetComponentInChildren<Tanks.Lobby.ClientPaymentGUI.Impl.SpecialOfferDataProvider>();
        }

        public string SpecialOfferOneShotMessage { get; set; }

        public string SpecialOfferEmptyRewardMessage { get; set; }

        public Tanks.Lobby.ClientPaymentGUI.Impl.XCrystalsDataProvider XCrystalsDataProvider =>
            this.xCrystalsDataProvider;

        public Tanks.Lobby.ClientPaymentGUI.Impl.SpecialOfferDataProvider SpecialOfferDataProvider =>
            this.specialOfferDataProvider;
    }
}

