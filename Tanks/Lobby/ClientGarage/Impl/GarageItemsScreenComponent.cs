namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class GarageItemsScreenComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject buyButton;
        [SerializeField]
        private GameObject xBuyButton;
        [SerializeField]
        private MountLabelComponent mountLabel;
        [SerializeField]
        private MountItemButtonComponent mountItemButton;
        [SerializeField]
        private ItemPropertiesButtonComponent itemPropertiesButton;
        [SerializeField]
        private UserRankRestrictionDescriptionGUIComponent userRankRestrictionDescription;
        [SerializeField]
        private UpgradeLevelRestrictionDescriptionGUIComponent upgradeLevelRestrictionDescription;
        [SerializeField]
        private Text onlyInContainerLabel;
        [SerializeField]
        private GoToContainersScreenButtonComponent containersButton;

        public GameObject BuyButton =>
            this.buyButton;

        public GameObject XBuyButton =>
            this.xBuyButton;

        public MountLabelComponent MountLabel =>
            this.mountLabel;

        public MountItemButtonComponent MountItemButton =>
            this.mountItemButton;

        public ItemPropertiesButtonComponent ItemPropertiesButton =>
            this.itemPropertiesButton;

        public UserRankRestrictionDescriptionGUIComponent UserRankRestrictionDescription =>
            this.userRankRestrictionDescription;

        public UpgradeLevelRestrictionDescriptionGUIComponent UpgradeLevelRestrictionDescription =>
            this.upgradeLevelRestrictionDescription;

        public bool OnlyInContainerUIVisibility
        {
            set
            {
                this.onlyInContainerLabel.gameObject.SetActive(value);
                this.containersButton.gameObject.SetActive(value);
            }
        }

        public bool OnlyInContainerLabelVisibility
        {
            set => 
                this.onlyInContainerLabel.gameObject.SetActive(value);
        }

        public bool InContainerButtonVisibility
        {
            set => 
                this.containersButton.gameObject.SetActive(value);
        }

        public string OnlyInContainerLabel
        {
            set => 
                this.onlyInContainerLabel.text = value;
        }
    }
}

