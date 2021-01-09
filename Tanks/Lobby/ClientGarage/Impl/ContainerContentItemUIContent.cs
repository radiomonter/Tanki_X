namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using TMPro;
    using UnityEngine;

    public class ContainerContentItemUIContent : MonoBehaviour, ListItemContent
    {
        private GarageItem _item;
        [SerializeField]
        private TextMeshProUGUI _name;
        [SerializeField]
        private GameObject _own;
        [SerializeField]
        private ImageSkin _preview;
        [SerializeField]
        private LocalizedField _commonString;
        [SerializeField]
        private LocalizedField _rareString;
        [SerializeField]
        private LocalizedField _epicString;
        [SerializeField]
        private LocalizedField _legendaryString;

        public void Select()
        {
            VisualItem item = this._item as VisualItem;
            ContainersUI componentInParent = base.GetComponentInParent<ContainersUI>();
            base.GetComponentInParent<ContainerContentUI>().GraffitiRoot.SetActive(((item != null) && (item.Type == VisualItem.VisualItemType.Graffiti)) && componentInParent.previewMode);
            if (item != null)
            {
                ResetPreviewEvent evt = new ResetPreviewEvent {
                    ExceptPreviewGroup = this._item.MarketItem.GetComponent<PreviewGroupComponent>().Key
                };
                this.SendEvent<ResetPreviewEvent>(evt, null);
            }
            Entity userItem = this._item.UserItem;
            Entity entity = userItem;
            if (userItem == null)
            {
                Entity local1 = userItem;
                entity = this._item.MarketItem;
            }
            this.SendEvent<ListItemSelectedEvent>(entity);
        }

        public void SetDataProvider(object dataProvider)
        {
            if (!ReferenceEquals(this._item, dataProvider))
            {
                this._item = dataProvider as GarageItem;
                if (this._item != null)
                {
                    this.SetNameTo(this._name, this._item);
                    this._own.SetActive(!ReferenceEquals(this._item.UserItem, null));
                    this._preview.SpriteUid = this._item.Preview;
                    RectTransform component = this._preview.GetComponent<RectTransform>();
                    if (dataProvider is PremiumItem)
                    {
                        component.anchoredPosition = Vector2.zero;
                        this._preview.GetComponent<Image>().SetNativeSize();
                    }
                    else
                    {
                        VisualItem item = dataProvider as VisualItem;
                        if (item != null)
                        {
                            if ((item.Type != VisualItem.VisualItemType.Paint) && (item.Type != VisualItem.VisualItemType.Coating))
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
                }
            }
        }

        private void SetNameTo(TextMeshProUGUI tmpName, GarageItem newItem)
        {
            ContainerContentUI componentInParent = base.GetComponentInParent<ContainerContentUI>();
            string categoryName = MarketItemNameLocalization.Instance.GetCategoryName(newItem.MarketItem);
            string localizedContentItemName = string.Empty;
            if (newItem.MarketItem != null)
            {
                localizedContentItemName = componentInParent.Item.GetLocalizedContentItemName(newItem.MarketItem.Id);
            }
            if (string.IsNullOrEmpty(localizedContentItemName) || !string.IsNullOrEmpty(categoryName))
            {
                localizedContentItemName = $"{categoryName} {MarketItemNameLocalization.Instance.GetGarageItemName(this._item)}";
            }
            tmpName.text = localizedContentItemName;
            tmpName.color = newItem.Rarity.GetRarityColor();
            base.GetComponentInParent<TooltipShowBehaviour>().TipText = MarketItemNameLocalization.GetFullItemDescription(newItem, true, (string) this._commonString, (string) this._rareString, (string) this._epicString, (string) this._legendaryString);
        }

        public void UpdateOwn()
        {
            if (this._item != null)
            {
                this._own.SetActive(!ReferenceEquals(this._item.UserItem, null));
            }
        }
    }
}

