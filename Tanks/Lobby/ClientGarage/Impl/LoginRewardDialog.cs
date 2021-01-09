namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class LoginRewardDialog : ConfirmDialogComponent
    {
        public RectTransform itemsContainer;
        public ReleaseGiftItemComponent itemPrefab;
        public float itemsShowDelay = 0.6f;
        public ImageSkin leagueIcon;
        public TextMeshProUGUI headerText;
        public TextMeshProUGUI text;
        public LoginRewardAllItemsContainer allItems;
        public List<Entity> marketItems = new List<Entity>();
        [SerializeField]
        private LocalizedField paint;
        [SerializeField]
        private LocalizedField coating;
        [SerializeField]
        private LocalizedField dayShort;
        [SerializeField]
        private LocalizedField container;
        [SerializeField]
        private LocalizedField premium;

        public string GetRewardItemName(Entity marketItemEntity)
        {
            string name = marketItemEntity.GetComponent<DescriptionItemComponent>().Name;
            if (marketItemEntity.HasComponent<WeaponPaintItemComponent>())
            {
                name = this.coating.Value + "\n" + name;
            }
            else if (marketItemEntity.HasComponent<PaintItemComponent>())
            {
                name = this.paint.Value + "\n" + name;
            }
            else if (marketItemEntity.HasComponent<ContainerMarkerComponent>())
            {
                name = this.container.Value + "\n" + name;
            }
            else if (marketItemEntity.HasComponent<PremiumBoostItemComponent>())
            {
                name = this.premium.Value + " {0} " + this.dayShort.Value;
            }
            return name;
        }

        public string GetRewardItemNameWithAmount(Entity marketItemEntity, int amount)
        {
            string name = marketItemEntity.GetComponent<DescriptionItemComponent>().Name;
            if (marketItemEntity.HasComponent<WeaponPaintItemComponent>())
            {
                name = this.coating.Value + " " + name;
            }
            else if (marketItemEntity.HasComponent<PaintItemComponent>())
            {
                name = this.paint.Value + " " + name;
            }
            else if (marketItemEntity.HasComponent<PremiumBoostItemComponent>())
            {
                object[] objArray1 = new object[] { this.premium.Value, " ", amount, " ", this.dayShort.Value };
                name = string.Concat(objArray1);
            }
            else if (!marketItemEntity.HasComponent<ContainerMarkerComponent>())
            {
                name = name + " x" + amount;
            }
            else
            {
                object[] objArray2 = new object[] { this.container.Value, "\n", name, " x", amount };
                name = string.Concat(objArray2);
            }
            return name;
        }

        public void ScrollToCurrentDay()
        {
            this.allItems.ScrollToCurrentDay();
        }
    }
}

