namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using TMPro;
    using UnityEngine;

    public class ContainerBonusView : MonoBehaviour
    {
        public ImageSkin imageSkin;
        public TextMeshProUGUI text;
        public GameObject back;

        public void UpdateView(DailyBonusGarageItemReward containerItem)
        {
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(containerItem.MarketItemId);
            this.imageSkin.SpriteUid = item.Preview;
            this.text.text = item.Name.ToUpper();
            this.back.SetActive(true);
        }

        public void UpdateViewByMarketItem(long marketItem)
        {
            GarageItem item = GarageItemsRegistry.GetItem<GarageItem>(marketItem);
            this.imageSkin.SpriteUid = item.Preview;
            this.text.text = item.Name.ToUpper();
            this.back.SetActive(true);
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }
    }
}

