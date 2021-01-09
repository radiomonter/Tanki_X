namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class DetailTargetTeleportView : MonoBehaviour
    {
        public ImageSkin[] imageSkins;
        public TextMeshProUGUI text;
        public BrokenBackView back;
        public Image fill;

        public void UpdateView(DailyBonusGarageItemReward detailMarketItem)
        {
            <UpdateView>c__AnonStorey0 storey = new <UpdateView>c__AnonStorey0 {
                detailItem = GarageItemsRegistry.GetItem<DetailItem>(detailMarketItem.MarketItemId)
            };
            this.imageSkins.ForEach<ImageSkin>(new Action<ImageSkin>(storey.<>m__0));
            this.text.text = storey.detailItem.TargetMarketItem.Name;
            this.fill.gameObject.SetActive(true);
            this.fill.fillAmount = 1f;
            this.back.BreakBack();
            this.back.enabled = true;
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        [CompilerGenerated]
        private sealed class <UpdateView>c__AnonStorey0
        {
            internal DetailItem detailItem;

            internal void <>m__0(ImageSkin i)
            {
                i.SpriteUid = this.detailItem.TargetMarketItem.Preview;
            }
        }
    }
}

