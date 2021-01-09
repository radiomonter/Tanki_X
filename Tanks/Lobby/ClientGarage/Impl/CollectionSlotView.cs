namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
    using TMPro;
    using UnityEngine;

    public class CollectionSlotView : SlotView
    {
        public ImageSkin moduleIcon;
        public TextMeshProUGUI improveAvailableText;
        public TextMeshProUGUI researchAvailableText;
        public TextMeshProUGUI cardCountText;
        public Color yelloColor;
        public SlotInteractive interactive;
        public Action<CollectionSlotView> onClick;
        public Action<CollectionSlotView> onDoubleClick;
        private tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items.ModuleItem moduleItem;

        public void Deselect()
        {
            this.interactive.Deselect();
        }

        public void Init(tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items.ModuleItem moduleItem)
        {
            this.moduleItem = moduleItem;
            this.moduleIcon.SpriteUid = moduleItem.CardSpriteUid;
            this.interactive.moduleItem = moduleItem;
            this.interactive.onClick = () => this.onClick(this);
            this.interactive.onDoubleClick = () => this.onDoubleClick(this);
            base.tooltip.SetCustomContentData(moduleItem);
            this.UpdateView();
        }

        public void Select()
        {
            this.interactive.Select();
        }

        public void UpdateView()
        {
            this.cardCountText.gameObject.SetActive(false);
            this.researchAvailableText.gameObject.SetActive(false);
            this.improveAvailableText.gameObject.SetActive(false);
            this.moduleIcon.GetComponent<Image>().color = Color.white;
            Color white = Color.white;
            if (!ReferenceEquals(this.moduleItem.UserItem, null))
            {
                if (this.moduleItem.ImproveAvailable())
                {
                    this.improveAvailableText.gameObject.SetActive(true);
                    this.moduleIcon.GetComponent<Image>().color = this.yelloColor;
                    white = this.yelloColor;
                }
            }
            else
            {
                this.cardCountText.gameObject.SetActive(true);
                this.cardCountText.text = this.moduleItem.UserCardCount + "/" + this.moduleItem.CraftPrice.Cards;
                if (this.moduleItem.ResearchAvailable())
                {
                    this.researchAvailableText.gameObject.SetActive(true);
                    this.moduleIcon.GetComponent<Image>().color = this.yelloColor;
                    white = this.yelloColor;
                }
            }
            this.interactive.UpdateView(white);
        }

        public tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items.ModuleItem ModuleItem =>
            this.moduleItem;
    }
}

