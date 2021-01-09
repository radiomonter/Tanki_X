namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class GraphicsSettingsScreenComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject applyButton;
        [SerializeField]
        private GameObject cancelButton;
        [SerializeField]
        private GameObject defaultButton;
        [SerializeField]
        private TextMeshProUGUI reloadText;
        [SerializeField]
        private TextMeshProUGUI perfomanceChangeText;
        [SerializeField]
        private TextMeshProUGUI currentPerfomanceText;
        private bool needToReloadApplication;

        public void SetDefaultButtonVisibility(bool needToShow)
        {
            this.defaultButton.gameObject.SetActive(needToShow);
        }

        public void SetPerfomanceWarningVisibility(bool needToShowChangePerfomance, bool isCurrentQuality)
        {
            this.perfomanceChangeText.gameObject.SetActive(!isCurrentQuality && needToShowChangePerfomance);
            this.currentPerfomanceText.gameObject.SetActive(isCurrentQuality && needToShowChangePerfomance);
        }

        public void SetVisibilityForChangeSettingsControls(bool needToShowReload, bool needToShowButtons)
        {
            this.applyButton.gameObject.SetActive(needToShowButtons);
            this.cancelButton.gameObject.SetActive(needToShowButtons);
            this.NeedToReloadApplication = needToShowReload;
        }

        public bool NeedToReloadApplication
        {
            get => 
                this.needToReloadApplication;
            set
            {
                this.needToReloadApplication = value;
                this.reloadText.gameObject.SetActive(this.needToReloadApplication);
            }
        }
    }
}

