namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class ContainersScreenComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject openButton;
        [SerializeField]
        private GameObject openAllButton;
        [SerializeField]
        private GameObject rightPanel;
        [SerializeField]
        private GameObject emptyListText;
        [SerializeField]
        private GameObject contentButton;

        public void SetOpenButtonsActive(bool openActivity, bool openAllActivity)
        {
            this.openButton.SetActive(openActivity);
            this.openButton.SetInteractable(openActivity);
            this.openAllButton.SetActive(openAllActivity);
            this.openAllButton.SetInteractable(openAllActivity);
        }

        public void SetOpenButtonsInteractable(bool interactable)
        {
            this.openButton.SetInteractable(interactable);
            this.openAllButton.SetInteractable(interactable);
        }

        public GameObject OpenButton =>
            this.openButton;

        public GameObject OpenAllButton =>
            this.openAllButton;

        public bool OpenButtonActivity
        {
            get => 
                this.openButton.activeSelf;
            set
            {
                this.openButton.SetActive(value);
                this.openButton.SetInteractable(value);
            }
        }

        public GameObject RightPanel =>
            this.rightPanel;

        public GameObject EmptyListText =>
            this.emptyListText;

        public bool ContentButtonActivity
        {
            get => 
                this.contentButton.activeSelf;
            set
            {
                this.contentButton.SetActive(value);
                this.contentButton.SetInteractable(value);
            }
        }
    }
}

