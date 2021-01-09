namespace Tanks.Lobby.ClientLoading.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class LoadBundlesTaskProviderComponent : MonoBehaviour, Component
    {
        private LoadBundlesTaskComponent loadBundlesTask;
        public Action<LoadBundlesTaskComponent> OnDataChange;

        public void UpdateData(LoadBundlesTaskComponent loadBundlesTask)
        {
            this.loadBundlesTask = loadBundlesTask;
            if (this.OnDataChange != null)
            {
                this.OnDataChange(loadBundlesTask);
            }
        }

        public LoadBundlesTaskComponent LoadBundlesTask =>
            this.loadBundlesTask;
    }
}

