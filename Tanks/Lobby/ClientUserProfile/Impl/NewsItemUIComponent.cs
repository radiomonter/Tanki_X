namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class NewsItemUIComponent : UIBehaviour, Component
    {
        [SerializeField]
        private Text headerText;
        [SerializeField]
        private Text dateText;
        [SerializeField]
        private NewsImageContainerComponent imageContainer;
        [SerializeField]
        private RectTransform centralIconTransform;
        [SerializeField]
        private RectTransform saleIconTransform;
        [SerializeField]
        private Text saleIconText;
        [SerializeField]
        private GameObject border;
        public bool noSwap;
        private string tooltip = string.Empty;

        public void SetCentralIcon(Texture2D texture)
        {
            RawImage image = this.centralIconTransform.gameObject.AddComponent<RawImage>();
            image.texture = texture;
            image.SetNativeSize();
        }

        public string HeaderText
        {
            get => 
                this.headerText.text;
            set => 
                this.headerText.text = value;
        }

        public string DateText
        {
            get => 
                this.dateText.text;
            set => 
                this.dateText.text = value;
        }

        public bool SaleIconVisible
        {
            get => 
                this.saleIconTransform.gameObject.activeSelf;
            set => 
                this.saleIconTransform.gameObject.SetActive(value);
        }

        public string SaleIconText
        {
            get => 
                this.saleIconText.text;
            set => 
                this.saleIconText.text = value;
        }

        public string Tooltip
        {
            get => 
                this.tooltip;
            set
            {
                this.tooltip = value;
                TooltipShowBehaviour component = base.GetComponent<TooltipShowBehaviour>();
                if (component != null)
                {
                    component.TipText = this.tooltip;
                }
            }
        }

        public NewsImageContainerComponent ImageContainer =>
            this.imageContainer;

        public GameObject Border =>
            this.border;
    }
}

