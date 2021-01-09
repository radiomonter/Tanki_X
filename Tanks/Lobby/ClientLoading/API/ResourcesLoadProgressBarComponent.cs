namespace Tanks.Lobby.ClientLoading.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class ResourcesLoadProgressBarComponent : MonoBehaviour, Component
    {
        public static float PROGRESS_VISUAL_KOEFF = 0.95f;
        [SerializeField]
        private float timeBeforeProgressCalculation = 0.1f;
        [SerializeField]
        private float timeToFakeLoad = 2f;
        [SerializeField]
        private float bytesToFakeLoad = 1048576f;
        public LoadProgressBarView ProgressBar;

        private void OnEnable()
        {
            this.ProgressBar.ProgressValue = 0f;
        }

        public void UpdateView(LoadBundlesTaskComponent loadBundlesTask)
        {
            float num3 = Mathf.Clamp((float) (Mathf.Clamp(Time.realtimeSinceStartup - loadBundlesTask.LoadingStartTime, 0f, this.TimeToFakeLoad) / this.TimeToFakeLoad), (float) 0f, (float) 1f) * this.BytesToFakeLoad;
            float num4 = 0f;
            num4 = (loadBundlesTask.BytesToLoad <= 0) ? (num3 / this.BytesToFakeLoad) : ((loadBundlesTask.BytesLoaded + num3) / (loadBundlesTask.BytesToLoad + this.BytesToFakeLoad));
            float num5 = num4 * PROGRESS_VISUAL_KOEFF;
            if (this.ProgressBar.ProgressValue < num5)
            {
                this.ProgressBar.ProgressValue = num4 * PROGRESS_VISUAL_KOEFF;
            }
            if (loadBundlesTask.AllBundlesLoaded())
            {
                this.ProgressBar.ProgressValue = 1f;
            }
        }

        public float TimeBeforeProgressCalculation
        {
            get => 
                this.timeBeforeProgressCalculation;
            set => 
                this.timeBeforeProgressCalculation = value;
        }

        public float TimeToFakeLoad
        {
            get => 
                this.timeToFakeLoad;
            set => 
                this.timeToFakeLoad = value;
        }

        public float BytesToFakeLoad
        {
            get => 
                this.bytesToFakeLoad;
            set => 
                this.bytesToFakeLoad = value;
        }
    }
}

