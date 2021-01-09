namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class ModuleInfoPanelComponent : LocalizedControl, Component
    {
        [SerializeField]
        private Text slotText;
        [SerializeField]
        private Text moduleNameText;
        [SerializeField]
        private Text mountLabelText;
        [SerializeField]
        private RectTransform slotInfoPanel;
        [SerializeField]
        private ImageSkin slotInfoSlotIcon;
        [SerializeField]
        private ImageSkin slotInfoModuleIcon;
        [SerializeField]
        private ImageSkin slotInfoLockIcon;
        [SerializeField]
        private Text moduleDescriptionText;
        [SerializeField]
        private CardPriceLabelComponent priceLabel;
        [SerializeField]
        private Text moduleExceptionalText;
        [SerializeField]
        private Text moduleEpicText;
        [SerializeField]
        private GameObject defenceIcon;
        [SerializeField]
        private GameObject scoutingIcon;
        [SerializeField]
        private GameObject specialIcon;
        [SerializeField]
        private GameObject supportIcon;
        [Header("Localization"), SerializeField]
        private Text specialText;
        [SerializeField]
        private Text scoutingText;
        [SerializeField]
        private Text defenceText;
        [SerializeField]
        private Text supportText;

        public void ScrollUpDescription()
        {
            Vector2 vector = new Vector2();
            ((RectTransform) this.moduleDescriptionText.transform).anchoredPosition = vector;
        }

        public string SlotText
        {
            set => 
                this.slotText.text = value;
        }

        public string ModuleNameText
        {
            set => 
                this.moduleNameText.text = value;
        }

        public string MountLabelText
        {
            set => 
                this.mountLabelText.text = value;
        }

        public RectTransform SlotInfoPanel =>
            this.slotInfoPanel;

        public ImageSkin SlotInfoSlotIcon =>
            this.slotInfoSlotIcon;

        public ImageSkin SlotInfoModuleIcon =>
            this.slotInfoModuleIcon;

        public ImageSkin SlotInfoLockIcon =>
            this.slotInfoLockIcon;

        public string ModuleDescriptionText
        {
            set => 
                this.moduleDescriptionText.text = value;
        }

        public CardPriceLabelComponent PriceLabel =>
            this.priceLabel;

        public Text ModuleExceptionalText =>
            this.moduleExceptionalText;

        public Text ModuleEpicText =>
            this.moduleEpicText;

        public GameObject DefenceIcon =>
            this.defenceIcon;

        public GameObject ScoutingIcon =>
            this.scoutingIcon;

        public GameObject SpecialIcon =>
            this.specialIcon;

        public GameObject SupportIcon =>
            this.supportIcon;

        public string SpecialText
        {
            get => 
                this.specialText.text;
            set => 
                this.specialText.text = value;
        }

        public string ScoutingText
        {
            get => 
                this.scoutingText.text;
            set => 
                this.scoutingText.text = value;
        }

        public string DefenceText
        {
            get => 
                this.defenceText.text;
            set => 
                this.defenceText.text = value;
        }

        public string SupportText
        {
            get => 
                this.supportText.text;
            set => 
                this.supportText.text = value;
        }
    }
}

