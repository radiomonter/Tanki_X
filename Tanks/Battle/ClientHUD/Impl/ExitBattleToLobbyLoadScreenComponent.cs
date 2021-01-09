namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientLoading.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class ExitBattleToLobbyLoadScreenComponent : BehaviourComponent, NoScaleScreen
    {
        public ResourcesLoadProgressBarComponent progressBar;

        private void Awake()
        {
            LoadBundlesTaskProviderComponent component = base.GetComponent<LoadBundlesTaskProviderComponent>();
            component.OnDataChange += new Action<LoadBundlesTaskComponent>(this.OnDataChange);
        }

        private void OnDataChange(LoadBundlesTaskComponent loadBundlesTask)
        {
            this.progressBar.UpdateView(loadBundlesTask);
        }
    }
}

