namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class UserExperienceProgressBarComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private ProgressBar progressBar;

        public void SetProgress(float progressValue)
        {
            this.progressBar.ProgressValue = progressValue;
        }
    }
}

