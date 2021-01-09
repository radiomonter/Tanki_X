namespace Tanks.Lobby.ClientNavigation.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class TopPanelComponent : MonoBehaviour, Component
    {
        public GameObject backButton;
        public Image background;
        public Animator screenHeader;
        [SerializeField]
        private Text newHeaderText;
        [SerializeField]
        private Text currentHeaderText;
        private bool hasHeader;

        public void SetHeaderText(string headerText)
        {
            if (!this.hasHeader)
            {
                this.SetHeaderTextImmediately(headerText);
                this.hasHeader = true;
            }
            else
            {
                this.NewHeader = headerText;
                if (!this.screenHeader.isInitialized || !this.screenHeader.GetCurrentAnimatorStateInfo(0).IsName("Default"))
                {
                    this.CurrentHeader = headerText;
                }
            }
        }

        public void SetHeaderTextImmediately(string headerText)
        {
            this.NewHeader = headerText;
            this.CurrentHeader = headerText;
        }

        public string NewHeader
        {
            get => 
                this.newHeaderText.text;
            set => 
                this.newHeaderText.text = value.ToUpper();
        }

        public string CurrentHeader
        {
            get => 
                this.currentHeaderText.text;
            set => 
                this.currentHeaderText.text = value.ToUpper();
        }

        public bool HasHeader =>
            this.hasHeader && this.screenHeader.isInitialized;
    }
}

