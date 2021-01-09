namespace Tanks.Lobby.ClientLoading.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using TMPro;
    using UnityEngine;

    public class LobbyLoadScreenComponent : MonoBehaviour, Component, NoScaleScreen
    {
        public TextMeshProUGUI initialization;
        public ResourcesLoadProgressBarComponent progressBar;
        public LoadingStatusView loadingStatus;

        private void Awake()
        {
            base.GetComponent<LoadBundlesTaskProviderComponent>().OnDataChange = new Action<LoadBundlesTaskComponent>(this.OnDataChange);
        }

        private void OnDataChange(LoadBundlesTaskComponent loadBundlesTask)
        {
            this.progressBar.UpdateView(loadBundlesTask);
            this.initialization.gameObject.SetActive(loadBundlesTask.BytesToLoad <= loadBundlesTask.BytesLoaded);
            this.loadingStatus.UpdateView(loadBundlesTask);
        }
    }
}

