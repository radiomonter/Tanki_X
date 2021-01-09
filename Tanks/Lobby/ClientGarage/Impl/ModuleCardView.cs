namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class ModuleCardView : MonoBehaviour
    {
        [SerializeField]
        private Material[] cardMaterial;
        [SerializeField]
        private TextMeshProUGUI moduleLevel;
        [SerializeField]
        private TextMeshProUGUI moduleName;
        [SerializeField]
        private TextMeshProUGUI moduleCount;
        [SerializeField]
        private Color[] tierColor;
        [SerializeField]
        private Image background;
        public ImageSkin[] imageSkins;
        public Sprite[] tierBackgrounds;
        public MeshRenderer meshRenderer;
        private readonly StringBuilder stringBuilder = new StringBuilder(20);
        public int tierNumber;

        private void SetMaterial(int tier)
        {
            this.meshRenderer.sharedMaterial = this.cardMaterial[tier];
            this.background.sprite = this.tierBackgrounds[tier];
        }

        public void UpdateView(long moduleMarketItemId, long upgradeLevel = -1L, bool showName = true, bool showCount = true)
        {
            <UpdateView>c__AnonStorey0 storey = new <UpdateView>c__AnonStorey0 {
                $this = this
            };
            base.gameObject.SetActive(true);
            storey.moduleItem = GarageItemsRegistry.GetItem<ModuleItem>(moduleMarketItemId);
            this.imageSkins.ForEach<ImageSkin>(new Action<ImageSkin>(storey.<>m__0));
            this.imageSkins.ForEach<ImageSkin>(new Action<ImageSkin>(storey.<>m__1));
            this.SetMaterial(storey.moduleItem.TierNumber);
            this.tierNumber = storey.moduleItem.TierNumber;
            if (!showCount)
            {
                this.moduleCount.text = string.Empty;
            }
            else
            {
                this.stringBuilder.Length = 0;
                if (storey.moduleItem.UserItem != null)
                {
                    this.stringBuilder.AppendFormat(" {0}/{1}", storey.moduleItem.UserCardCount, storey.moduleItem.UpgradePrice);
                }
                else
                {
                    this.stringBuilder.AppendFormat(" {0}/{1}", storey.moduleItem.UserCardCount, storey.moduleItem.CraftPrice);
                }
                this.moduleCount.text = this.stringBuilder.ToString();
            }
            if (upgradeLevel == -1L)
            {
                upgradeLevel = storey.moduleItem.Level;
            }
            this.moduleLevel.text = (storey.moduleItem.UserItem == null) ? $"{upgradeLevel}" : $"{(upgradeLevel + 1L)}";
            base.name = storey.moduleItem.Name;
            this.moduleName.text = !showName ? string.Empty : $"{base.name}";
        }

        public void UpdateViewForCrystal(string spriteUid, string name)
        {
            <UpdateViewForCrystal>c__AnonStorey1 storey = new <UpdateViewForCrystal>c__AnonStorey1 {
                spriteUid = spriteUid,
                $this = this
            };
            base.gameObject.SetActive(true);
            this.imageSkins.ForEach<ImageSkin>(new Action<ImageSkin>(storey.<>m__0));
            this.imageSkins.ForEach<ImageSkin>(new Action<ImageSkin>(storey.<>m__1));
            this.SetMaterial(0);
            this.stringBuilder.Length = 0;
            this.moduleName.text = $"{name}";
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        [CompilerGenerated]
        private sealed class <UpdateView>c__AnonStorey0
        {
            internal ModuleItem moduleItem;
            internal ModuleCardView $this;

            internal void <>m__0(ImageSkin i)
            {
                i.SpriteUid = this.moduleItem.CardSpriteUid;
            }

            internal void <>m__1(ImageSkin i)
            {
                i.transform.gameObject.GetComponent<Image>().color = this.$this.tierColor[this.moduleItem.TierNumber];
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateViewForCrystal>c__AnonStorey1
        {
            internal string spriteUid;
            internal ModuleCardView $this;

            internal void <>m__0(ImageSkin i)
            {
                i.SpriteUid = this.spriteUid;
            }

            internal void <>m__1(ImageSkin i)
            {
                i.transform.gameObject.GetComponent<Image>().color = this.$this.tierColor[0];
            }
        }
    }
}

