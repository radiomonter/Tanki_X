namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class DailyBonusInfoScreen : ConfirmDialogComponent
    {
        [SerializeField]
        private DailyBonusScreenComponent dailyBonusScreen;

        public override void Hide()
        {
            base.Hide();
            MainScreenComponent.Instance.ClearOnBackOverride();
            this.dailyBonusScreen.Show();
        }

        public void Show()
        {
            this.dailyBonusScreen.Hide();
            MainScreenComponent.Instance.OverrideOnBack(new Action(this.Hide));
            base.Show(null);
        }

        private void Update()
        {
            if (InputMapping.Cancel)
            {
                this.Hide();
            }
        }
    }
}

