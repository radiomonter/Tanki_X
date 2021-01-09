namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientNavigation.API;

    public class ProfileTabManager : TabManager
    {
        protected override void OnDisable()
        {
            base.OnDisable();
            base.index = 0;
        }

        protected override void OnEnable()
        {
            this.Show(base.index);
        }

        public override void Show(int newIndex)
        {
            base.Show(newIndex);
            LogScreen screen = (newIndex == 1) ? LogScreen.ProfileAccount : LogScreen.ProfileSummary;
            MainScreenComponent.Instance.SendShowScreenStat(screen);
        }
    }
}

