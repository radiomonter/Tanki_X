namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API.List;
    using UnityEngine;

    public class DynamicVerticalList : MonoBehaviour
    {
        [SerializeField]
        private RectTransform item;
        [SerializeField]
        private RectTransform itemContent;
        [SerializeField]
        private int itemHeight;
        [SerializeField]
        private int spacing;
        [SerializeField]
        private RectTransform viewport;
        private ListDataProvider dataProvider;
        private List<ListItem> generatedItems = new List<ListItem>();
        private RectTransform rectTransform;
        private int visibleItemsCount;
        private ListItem selected;

        private void Awake()
        {
            this.rectTransform = base.GetComponent<RectTransform>();
        }

        private void CalculateVisibleItems()
        {
            this.visibleItemsCount = Math.Min((int) ((this.viewport.rect.height / ((float) (this.itemHeight + this.spacing))) + 2f), this.dataProvider.Data.Count);
            if (this.visibleItemsCount > this.generatedItems.Count)
            {
                for (int i = this.generatedItems.Count; i < this.visibleItemsCount; i++)
                {
                    RectTransform transform = Instantiate<RectTransform>(this.item);
                    ListItem component = transform.GetComponent<ListItem>();
                    component.SetContent(Instantiate<RectTransform>(this.itemContent));
                    this.generatedItems.Add(component);
                    transform.SetParent(this.rectTransform, false);
                    Vector2 vector = new Vector2(0f, 1f);
                    transform.anchorMin = vector;
                    transform.anchorMax = vector;
                    transform.pivot = new Vector2(0f, 1f);
                }
            }
        }

        private void Layout()
        {
            int num;
            if (((int) (this.rectTransform.anchoredPosition.y / ((float) (this.itemHeight + this.spacing)))) < 0)
            {
                Vector2 vector2 = new Vector2();
                this.rectTransform.anchoredPosition = vector2;
                num = 0;
            }
            for (int i = 0; (i < this.visibleItemsCount) && ((num + i) < this.dataProvider.Data.Count); i++)
            {
                ListItem item = this.generatedItems[i];
                if (!item.gameObject.activeSelf)
                {
                    item.gameObject.SetActive(true);
                }
                RectTransform component = item.GetComponent<RectTransform>();
                component.anchoredPosition = new Vector2(0f, (float) (-(num + i) * (this.itemHeight + this.spacing)));
                component.sizeDelta = new Vector2(this.viewport.rect.width, (float) this.itemHeight);
                item.Data = this.dataProvider.Data[num + i];
                if (item.Data == this.dataProvider.Selected)
                {
                    item.PlaySelectionAnimation();
                }
                else
                {
                    item.PlayDeselectionAnimation();
                }
            }
            for (int j = this.visibleItemsCount; j < this.generatedItems.Count; j++)
            {
                ListItem item2 = this.generatedItems[j];
                if (item2.gameObject.activeSelf)
                {
                    item2.gameObject.SetActive(false);
                }
            }
        }

        private void OnDisable()
        {
            this.dataProvider.DataChanged -= new Action<ListDataProvider>(this.UpdateBounds);
            this.rectTransform.anchoredPosition = new Vector2(0f, 0f);
            this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0f);
        }

        private void OnEnable()
        {
            this.dataProvider = base.GetComponent<ListDataProvider>();
            this.dataProvider ??= base.gameObject.AddComponent<DefaultListDataProvider>();
            this.dataProvider.DataChanged += new Action<ListDataProvider>(this.UpdateBounds);
            this.UpdateBounds(this.dataProvider);
        }

        private void OnItemSelect(ListItem listItem)
        {
            if (this.selected != null)
            {
                this.selected.PlayDeselectionAnimation();
            }
            this.selected = listItem;
        }

        public void ScrollToSelection()
        {
            float num;
            if (this.rectTransform.anchoredPosition.y < 0f)
            {
                Vector2 vector2 = new Vector2();
                this.rectTransform.anchoredPosition = vector2;
                num = 0f;
            }
            if (this.dataProvider.Selected != null)
            {
                int index = this.dataProvider.Data.IndexOf(this.dataProvider.Selected);
                if (index >= 0)
                {
                    int num3 = index * (this.itemHeight + this.spacing);
                    float num4 = num3 - num;
                    if (((num4 + this.itemHeight) > this.viewport.rect.height) || (num4 < 0f))
                    {
                        float y = Math.Min((float) num3, this.rectTransform.rect.height - this.viewport.rect.height);
                        this.rectTransform.anchoredPosition = new Vector2(0f, y);
                    }
                }
            }
        }

        private void Update()
        {
            this.CalculateVisibleItems();
            this.Layout();
        }

        private void UpdateBounds(ListDataProvider provider)
        {
            this.UpdateSize();
            if (provider.Selected != null)
            {
                this.ScrollToSelection();
                this.Layout();
                foreach (ListItem item in this.generatedItems)
                {
                    if (item.Data == provider.Selected)
                    {
                        item.Select(false);
                        break;
                    }
                }
            }
        }

        private void UpdateSize()
        {
            this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.viewport.rect.width - 1f);
            int num = (this.dataProvider.Data.Count * this.itemHeight) + ((this.dataProvider.Data.Count - 1) * this.spacing);
            this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float) num);
            if (num < this.rectTransform.anchoredPosition.y)
            {
                this.rectTransform.anchoredPosition = new Vector2(0f, Math.Min((float) 0f, (float) (num - this.viewport.rect.height)));
            }
        }
    }
}

