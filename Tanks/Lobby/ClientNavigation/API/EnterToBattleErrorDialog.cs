namespace Tanks.Lobby.ClientNavigation.API
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class EnterToBattleErrorDialog : NotificationDialogComponent
    {
        [SerializeField]
        private LocalizedField errorText;

        public void Show()
        {
            base.Show(this.errorText.Value);
        }
    }
}

