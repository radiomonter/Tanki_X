namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using TMPro;
    using UnityEngine;

    public class VisualItemUIContent : MonoBehaviour, ListItemContent
    {
        private static int EQUIPPED_STATE = Animator.StringToHash("Equipped");
        private static int INSTANT_STATE = Animator.StringToHash("Instant");
        [SerializeField]
        private TextMeshProUGUI name;
        [SerializeField]
        private ImageSkin preview;
        [SerializeField]
        private ListItemPrices prices;
        [SerializeField]
        private TextMeshProUGUI containerLabel;
        [SerializeField]
        private TextMeshProUGUI upgradesLabel;
        [SerializeField]
        private LocalizedField upgradesRequiredText;
        [SerializeField]
        private LocalizedField _commonString;
        [SerializeField]
        private LocalizedField _rareString;
        [SerializeField]
        private LocalizedField _epicString;
        [SerializeField]
        private LocalizedField _legendaryString;
        private VisualItem item;

        private int GetItemLevel(VisualItem visualItem) => 
            (visualItem.ParentItem == null) ? 0 : visualItem.ParentItem.UpgradeLevel;

        public void Select()
        {
            if (!this.item.IsSelected)
            {
                Entity userItem = this.item.UserItem;
                Entity entity = userItem;
                if (userItem == null)
                {
                    Entity local1 = userItem;
                    entity = this.item.MarketItem;
                }
                this.SendEvent<ListItemSelectedEvent>(entity);
            }
        }

        private void SendChanged()
        {
            if (this.item.IsSelected)
            {
                base.SendMessageUpwards("OnItemChanged", this.item);
            }
        }

        public void SetDataProvider(object dataProvider)
        {
            VisualItem newItem = (VisualItem) dataProvider;
            if (newItem.WaitForBuy && (newItem.UserItem != null))
            {
                newItem.WaitForBuy = false;
                this.SetNameTo(this.name, newItem);
                this.UpdatePrice(this.GetItemLevel(newItem));
                this.UpdateState(false);
            }
            else if (ReferenceEquals(newItem, this.item))
            {
                this.UpdatePrice(this.GetItemLevel(newItem));
                this.UpdateState(false);
            }
            else
            {
                this.item = newItem;
                this.SetNameTo(this.name, newItem);
                this.UpdatePrice(this.GetItemLevel(newItem));
                this.UpdateState(true);
                this.preview.SpriteUid = newItem.Preview;
                RectTransform component = this.preview.GetComponent<RectTransform>();
                if ((newItem.Type != VisualItem.VisualItemType.Paint) && (newItem.Type != VisualItem.VisualItemType.Coating))
                {
                    component.anchoredPosition = Vector2.zero;
                    component.sizeDelta = new Vector2(500f, 300f);
                }
                else
                {
                    component.anchoredPosition = new Vector2(-76f, -88f);
                    component.sizeDelta = new Vector2(1121f, 544f);
                }
            }
        }

        private void SetNameTo(TextMeshProUGUI tmpName, VisualItem newItem)
        {
            tmpName.text = newItem.Name;
            tmpName.color = newItem.Rarity.GetRarityColor();
            TooltipShowBehaviour componentInParent = base.GetComponentInParent<TooltipShowBehaviour>();
            if (componentInParent != null)
            {
                componentInParent.TipText = MarketItemNameLocalization.GetFullItemDescription(newItem, false, (string) this._commonString, (string) this._rareString, (string) this._epicString, (string) this._legendaryString);
            }
        }

        private void UpdatePrice(int currentLevel)
        {
            this.prices.Set(this.item);
            this.upgradesLabel.text = string.Empty;
            if (this.prices.gameObject.activeSelf)
            {
                this.containerLabel.gameObject.SetActive(false);
                this.upgradesLabel.gameObject.SetActive(false);
            }
            else
            {
                if (this.item.UserItem != null)
                {
                    this.containerLabel.gameObject.SetActive(false);
                    int restrictionLevel = this.item.RestrictionLevel;
                    if (this.item.IsRestricted)
                    {
                        this.upgradesLabel.gameObject.SetActive(restrictionLevel > currentLevel);
                        this.upgradesLabel.text = string.Format(this.upgradesRequiredText.Value, restrictionLevel);
                    }
                }
                else
                {
                    this.upgradesLabel.gameObject.SetActive(false);
                    this.containerLabel.gameObject.SetActive(this.item.IsContainerItem);
                    if (!this.item.IsContainerItem)
                    {
                        int restrictionLevel = this.item.RestrictionLevel;
                        if (this.item.IsRestricted)
                        {
                            this.upgradesLabel.gameObject.SetActive(restrictionLevel > currentLevel);
                            this.upgradesLabel.text = string.Format(this.upgradesRequiredText.Value, restrictionLevel);
                        }
                    }
                }
                if ((this.item.Type == VisualItem.VisualItemType.Graffiti) && ((this.item.ParentItem != null) && this.item.IsRestricted))
                {
                    int restrictionLevel = this.item.RestrictionLevel;
                    this.upgradesLabel.text = string.Format(this.upgradesRequiredText.Value, restrictionLevel);
                    this.upgradesLabel.text = this.upgradesLabel.text + $" ({this.item.ParentItem.Name})";
                }
            }
        }

        private void UpdateState(bool instant)
        {
            Animator component = base.GetComponent<Animator>();
            bool @bool = component.GetBool(EQUIPPED_STATE);
            if (this.item.UserItem == null)
            {
                if (@bool)
                {
                    component.SetBool(INSTANT_STATE, instant);
                    component.SetBool(EQUIPPED_STATE, false);
                }
            }
            else if (this.item.UserItem.HasComponent<MountedItemComponent>() && !@bool)
            {
                component.SetBool(INSTANT_STATE, instant);
                component.SetBool(EQUIPPED_STATE, true);
                this.SendChanged();
            }
            else if (!this.item.UserItem.HasComponent<MountedItemComponent>() && @bool)
            {
                component.SetBool(INSTANT_STATE, instant);
                component.SetBool(EQUIPPED_STATE, false);
                this.SendChanged();
            }
        }
    }
}

