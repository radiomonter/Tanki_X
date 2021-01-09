namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    public class FullEnergyDialog : NotificationDialogComponent
    {
        [SerializeField]
        private LocalizedField errorText;

        public void Show()
        {
            base.Show(this.errorText.Value);
        }
    }
}

