namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using TMPro;
    using UnityEngine;

    public class DetailBonusView : MonoBehaviour
    {
        public ImageSkin[] imageSkins;
        public TextMeshProUGUI text;
        private readonly StringBuilder stringBuilder = new StringBuilder(20);
        public GameObject brokenBack;
        private string mainText;

        private void Awake()
        {
            this.mainText = LocalizationUtils.Localize(this.text.GetComponent<TMPLocalize>().TextUid);
        }

        public void UpdateView(DailyBonusGarageItemReward detailMarketItem)
        {
            <UpdateView>c__AnonStorey0 storey = new <UpdateView>c__AnonStorey0 {
                detailItem = GarageItemsRegistry.GetItem<DetailItem>(detailMarketItem.MarketItemId)
            };
            this.imageSkins.ForEach<ImageSkin>(new Action<ImageSkin>(storey.<>m__0));
            this.stringBuilder.Length = 0;
            this.stringBuilder.Append(this.mainText);
            this.stringBuilder.AppendFormat(" {0}/{1}", storey.detailItem.Count + 1, storey.detailItem.RequiredCount);
            this.text.text = this.stringBuilder.ToString();
        }

        public void UpdateViewByMarketItem(long marketItem)
        {
            <UpdateViewByMarketItem>c__AnonStorey1 storey = new <UpdateViewByMarketItem>c__AnonStorey1 {
                detailItem = GarageItemsRegistry.GetItem<DetailItem>(marketItem)
            };
            this.imageSkins.ForEach<ImageSkin>(new Action<ImageSkin>(storey.<>m__0));
            this.stringBuilder.Length = 0;
            this.stringBuilder.Append(this.mainText);
            this.stringBuilder.AppendFormat(" {0}/{1}", storey.detailItem.Count + 1, storey.detailItem.RequiredCount);
            this.text.text = this.stringBuilder.ToString();
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        [CompilerGenerated]
        private sealed class <UpdateView>c__AnonStorey0
        {
            internal DetailItem detailItem;

            internal void <>m__0(ImageSkin i)
            {
                i.SpriteUid = this.detailItem.Preview;
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateViewByMarketItem>c__AnonStorey1
        {
            internal DetailItem detailItem;

            internal void <>m__0(ImageSkin i)
            {
                i.SpriteUid = this.detailItem.Preview;
            }
        }
    }
}

