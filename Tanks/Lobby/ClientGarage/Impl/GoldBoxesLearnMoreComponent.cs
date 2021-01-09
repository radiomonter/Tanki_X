namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientPaymentGUI.Impl;

    public class GoldBoxesLearnMoreComponent : ConfirmDialogComponent
    {
        private void Start()
        {
            GoToShopButton componentInChildren = base.GetComponentInChildren<GoToShopButton>();
            if (componentInChildren != null)
            {
                componentInChildren.DesiredShopTab = 6;
                componentInChildren.CallDialog = this;
            }
        }
    }
}

