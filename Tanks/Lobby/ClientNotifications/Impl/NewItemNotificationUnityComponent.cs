namespace Tanks.Lobby.ClientNotifications.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientGraphics.Impl;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class NewItemNotificationUnityComponent : BehaviourComponent
    {
        public Slider upgradeSlider;
        public AnimatedValueComponent upgradeAnimator;
        public int count;
        [SerializeField]
        private TextMeshProUGUI headerElement;
        [SerializeField]
        private GameObject containerContent;
        [SerializeField]
        private TextMeshProUGUI itemNameElement;
        [SerializeField]
        private ImageSkin itemIconSkin;
        [SerializeField]
        private ImageSkin resourceIconSkin;
        [SerializeField]
        private GameObject itemContent;
        [SerializeField]
        private GameObject resourceContent;
        [SerializeField]
        private Image borderImg;
        [SerializeField]
        private TextMeshProUGUI rarityNameElement;
        [SerializeField]
        private GameObject rareEffect;
        [SerializeField]
        private GameObject epicEffect;
        [SerializeField]
        private GameObject legendaryEffect;
        [SerializeField]
        private LocalizedField commonText;
        [SerializeField]
        private LocalizedField rareText;
        [SerializeField]
        private LocalizedField epicText;
        [SerializeField]
        private LocalizedField legendaryText;
        [SerializeField]
        public Material[] cardMaterial;
        public OutlineObject outline;
        public ModuleCardView view;
        [SerializeField]
        private GameObject cardElement;

        public void SetCardElement(int tier)
        {
            base.GetComponentInParent<LayoutElement>().preferredWidth = 300f;
            this.cardElement.SetActive(true);
            this.containerContent.SetActive(false);
            this.cardElement.GetComponent<Renderer>().sharedMaterial = this.cardMaterial[tier - 1];
        }

        public void SetItemIcon(string spriteUid)
        {
            this.resourceIconSkin.SpriteUid = spriteUid;
            this.resourceContent.SetActive(true);
        }

        public void SetItemImage(string spriteUid)
        {
            this.itemIconSkin.SpriteUid = spriteUid;
            this.itemContent.SetActive(true);
        }

        public void SetItemRarity(GarageItem item)
        {
            Color rarityColor = item.Rarity.GetRarityColor();
            this.itemNameElement.color = rarityColor;
            this.borderImg.color = rarityColor;
            this.rarityNameElement.color = new Color(rarityColor.r, rarityColor.g, rarityColor.b, 0.3f);
            if (!item.IsVisualItem)
            {
                this.rarityNameElement.gameObject.SetActive(false);
            }
            else
            {
                switch (item.Rarity)
                {
                    case ItemRarityType.COMMON:
                        this.rarityNameElement.text = $"[{this.commonText.Value}]";
                        break;

                    case ItemRarityType.RARE:
                        this.rarityNameElement.text = $"[{this.rareText.Value}]";
                        this.rareEffect.SetActive(true);
                        break;

                    case ItemRarityType.EPIC:
                        this.rarityNameElement.text = $"[{this.epicText.Value}]";
                        this.epicEffect.SetActive(true);
                        break;

                    case ItemRarityType.LEGENDARY:
                        this.rarityNameElement.text = $"[{this.legendaryText.Value}]";
                        this.legendaryEffect.SetActive(true);
                        break;

                    default:
                        break;
                }
            }
        }

        public TextMeshProUGUI HeaderElement =>
            this.headerElement;

        public TextMeshProUGUI ItemNameElement =>
            this.itemNameElement;

        public bool ContainerContent
        {
            set => 
                this.containerContent.SetActive(value);
        }
    }
}

