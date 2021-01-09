namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientPaymentGUI.Impl;

    public class PremiumLearnMoreComponent : ConfirmDialogComponent
    {
        public TabManager tabManager;
        public PremiumInfoUiElement[] uiElements;

        private void Start()
        {
            GoToShopButton componentInChildren = base.GetComponentInChildren<GoToShopButton>();
            if (componentInChildren != null)
            {
                componentInChildren.DesiredShopTab = 5;
                componentInChildren.CallDialog = this;
            }
        }
    }
}

