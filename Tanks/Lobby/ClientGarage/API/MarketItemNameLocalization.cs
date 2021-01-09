namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLocale.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class MarketItemNameLocalization : MonoBehaviour
    {
        public static MarketItemNameLocalization Instance;
        [SerializeField]
        private LocalizedField skin;
        [SerializeField]
        private LocalizedField coating;
        [SerializeField]
        private LocalizedField paint;
        [SerializeField]
        private LocalizedField container;
        [SerializeField]
        private LocalizedField graffity;
        [SerializeField]
        private LocalizedField shell;
        [SerializeField]
        private LocalizedField avatar;

        private void Awake()
        {
            Instance = this;
        }

        public string GetCategoryName(Entity entity) => 
            (entity != null) ? (!entity.HasComponent<SkinItemComponent>() ? (!entity.HasComponent<TankPaintItemComponent>() ? (!entity.HasComponent<WeaponPaintItemComponent>() ? (!entity.HasComponent<ContainerMarkerComponent>() ? (!entity.HasComponent<GraffitiItemComponent>() ? (!entity.HasComponent<ShellItemComponent>() ? (!entity.HasComponent<AvatarItemComponent>() ? string.Empty : this.avatar.Value) : this.shell.Value) : this.graffity.Value) : this.container.Value) : this.coating.Value) : this.paint.Value) : this.skin.Value) : string.Empty;

        public static string GetDetailedName(Entity marketItem)
        {
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(marketItem);
            string savedLocaleCode = LocaleUtils.GetSavedLocaleCode();
            return (!"ru".Equals(savedLocaleCode) ? (Instance.GetGarageItemName(item) + " " + Instance.GetCategoryName(marketItem)) : (Instance.GetCategoryName(marketItem) + " " + Instance.GetGarageItemName(item)));
        }

        public static string GetFullItemDescription(Entity marketItem, string commonRarity = "", string rareRarity = "", string epicRarity = "", string legendaryRarity = "")
        {
            string detailedName = GetDetailedName(marketItem);
            string str2 = string.Empty;
            ItemRarityType rarityType = marketItem.GetComponent<ItemRarityComponent>().RarityType;
            switch (rarityType)
            {
                case ItemRarityType.COMMON:
                    str2 = str2 + commonRarity;
                    break;

                case ItemRarityType.RARE:
                    str2 = str2 + rareRarity;
                    break;

                case ItemRarityType.EPIC:
                    str2 = str2 + epicRarity;
                    break;

                case ItemRarityType.LEGENDARY:
                    str2 = str2 + legendaryRarity;
                    break;

                default:
                    break;
            }
            if (!string.IsNullOrEmpty(str2))
            {
                string str3 = detailedName;
                string[] textArray1 = new string[] { str3, "\n<color=#", rarityType.GetRarityColor().ToHexString(), ">", str2, "</color>" };
                detailedName = string.Concat(textArray1);
            }
            string description = marketItem.GetComponent<DescriptionItemComponent>().Description;
            if (!string.IsNullOrEmpty(description))
            {
                detailedName = detailedName + "\n" + description;
            }
            return detailedName;
        }

        public static string GetFullItemDescription(GarageItem item, bool withParentItemName, string commonRarity = "", string rareRarity = "", string epicRarity = "", string legendaryRarity = "")
        {
            string str = !withParentItemName ? (Instance.GetCategoryName(item.MarketItem) + " " + item.Name) : GetDetailedName(item.MarketItem);
            string str2 = string.Empty;
            ItemRarityType rarity = item.Rarity;
            switch (item.Rarity)
            {
                case ItemRarityType.COMMON:
                    str2 = str2 + commonRarity;
                    break;

                case ItemRarityType.RARE:
                    str2 = str2 + rareRarity;
                    break;

                case ItemRarityType.EPIC:
                    str2 = str2 + epicRarity;
                    break;

                case ItemRarityType.LEGENDARY:
                    str2 = str2 + legendaryRarity;
                    break;

                default:
                    break;
            }
            if (!string.IsNullOrEmpty(str2))
            {
                string str3 = str;
                string[] textArray1 = new string[] { str3, "\n<color=#", rarity.GetRarityColor().ToHexString(), ">", str2, "</color>" };
                str = string.Concat(textArray1);
            }
            string description = item.Description;
            if (!string.IsNullOrEmpty(description))
            {
                str = str + "\n" + description;
            }
            return str;
        }

        public string GetGarageItemName(GarageItem item)
        {
            VisualItem item2 = item as VisualItem;
            return (((item2 == null) || ((item2.ParentItem == null) || item2.Name.Contains(item2.ParentItem.Name))) ? item.Name : $"{item2.ParentItem.Name} {item2.Name}");
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }
    }
}

