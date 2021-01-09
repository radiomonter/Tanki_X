namespace Tanks.Lobby.ClientLoading.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    public class PreloadAllResourcesScreenComponent : MonoBehaviour, Component, NoScaleScreen
    {
        public ResourcesLoadProgressBarComponent progressBar;
        public LoadingStatusView loadingStatusView;

        private void Awake()
        {
            base.GetComponent<LoadBundlesTaskProviderComponent>().OnDataChange = new Action<LoadBundlesTaskComponent>(this.OnDataChange);
        }

        private void OnDataChange(LoadBundlesTaskComponent loadBundlesTask)
        {
            this.progressBar.UpdateView(loadBundlesTask);
            this.loadingStatusView.UpdateView(loadBundlesTask);
        }
    }
}

