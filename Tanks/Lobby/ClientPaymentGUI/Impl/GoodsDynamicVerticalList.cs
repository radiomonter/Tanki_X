namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientControls.API.List;
    using UnityEngine;
    using UnityEngine.UI;

    public class GoodsDynamicVerticalList : MonoBehaviour
    {
        private const int commentSize = 200;
        [SerializeField]
        private GameObject commentPrefab;
        [SerializeField]
        private RectTransform item;
        [SerializeField]
        private List<GoodsContentAdapter> Adapters;
        [SerializeField]
        private int spacing;
        [SerializeField]
        private RectTransform viewport;
        private Dictionary<GoodsType, List<ListItem>> generatedItems = new Dictionary<GoodsType, List<ListItem>>();
        private List<Text> Comments = new List<Text>();
        private RectTransform rectTransform;
        private int visibleItemsCount;
        [CompilerGenerated]
        private static Predicate<GoodsContentAdapter> <>f__am$cache0;
        [CompilerGenerated]
        private static Predicate<GoodsContentAdapter> <>f__am$cache1;

        private void Awake()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = x => x.Type == GoodsType.XCrystals;
            }
            this.Adapters.Find(<>f__am$cache0).DataProvider = base.GetComponent<XCrystalsDataProvider>();
            <>f__am$cache1 ??= x => (x.Type == GoodsType.SpecialOffer);
            this.Adapters.Find(<>f__am$cache1).DataProvider = base.GetComponent<SpecialOfferDataProvider>();
            this.rectTransform = base.GetComponent<RectTransform>();
        }

        private void Layout()
        {
            float num2 = 0f;
            float num3 = 0f;
            int num4 = 0;
            int num5 = 0;
            foreach (GoodsContentAdapter adapter in this.Adapters)
            {
                if (!this.generatedItems.ContainsKey(adapter.Type))
                {
                    this.generatedItems[adapter.Type] = new List<ListItem>();
                }
                num4 = 0;
                foreach (object obj2 in adapter.DataProvider.Data)
                {
                    if ((num3 - this.start) > this.end)
                    {
                        break;
                    }
                    num2 = num3 + adapter.Content.Height;
                    if (num2 >= this.start)
                    {
                        if (num4 != this.generatedItems[adapter.Type].Count)
                        {
                            this.generatedItems[adapter.Type][num4].gameObject.SetActive(true);
                        }
                        else
                        {
                            RectTransform transform = Instantiate<RectTransform>(this.item);
                            ListItem item = transform.GetComponent<ListItem>();
                            item.SetContent(Instantiate<RectTransform>(adapter.Content.Prefab));
                            this.generatedItems[adapter.Type].Add(item);
                            transform.SetParent(this.rectTransform, false);
                            Vector2 vector = new Vector2(0f, 1f);
                            transform.anchorMin = vector;
                            transform.anchorMax = vector;
                            transform.pivot = new Vector2(0f, 1f);
                        }
                        RectTransform component = this.generatedItems[adapter.Type][num4].GetComponent<RectTransform>();
                        component.anchoredPosition = new Vector2(0f, -num3);
                        component.sizeDelta = new Vector2(this.viewport.rect.width, (float) adapter.Content.Height);
                        this.generatedItems[adapter.Type][num4].Data = obj2;
                        num4++;
                        if (adapter.DataProvider.HasComment(obj2))
                        {
                            if (num5 != this.Comments.Count)
                            {
                                this.Comments[num5].gameObject.SetActive(true);
                            }
                            else
                            {
                                Text item = Instantiate<GameObject>(this.commentPrefab).GetComponent<Text>();
                                item.transform.SetParent(this.rectTransform, false);
                                this.Comments.Add(item);
                            }
                            this.Comments[num5].text = adapter.DataProvider.GetComment(obj2);
                            RectTransform transform3 = this.Comments[num5].GetComponent<RectTransform>();
                            transform3.anchoredPosition = new Vector2(0f, -num2);
                            transform3.sizeDelta = new Vector2(this.viewport.rect.width, 200f);
                            num5++;
                            num2 += 200f;
                        }
                    }
                    num3 = num2 + this.spacing;
                }
                int num6 = num4;
                while (true)
                {
                    if (num6 >= this.generatedItems[adapter.Type].Count)
                    {
                        for (int i = num5; i < this.Comments.Count; i++)
                        {
                            this.Comments[i].gameObject.SetActive(false);
                        }
                        break;
                    }
                    this.generatedItems[adapter.Type][num6].gameObject.SetActive(false);
                    num6++;
                }
            }
        }

        private void OnDisable()
        {
            foreach (GoodsContentAdapter adapter in this.Adapters)
            {
                adapter.DataProvider.DataChanged -= new Action<ListDataProvider>(this.UpdateBounds);
            }
            this.rectTransform.anchoredPosition = new Vector2(0f, 0f);
            this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0f);
        }

        private void OnEnable()
        {
            foreach (GoodsContentAdapter adapter in this.Adapters)
            {
                adapter.DataProvider.DataChanged += new Action<ListDataProvider>(this.UpdateBounds);
            }
        }

        private void OnItemSelect(ListItem listItem)
        {
            foreach (GoodsContentAdapter adapter in this.Adapters)
            {
                Entity dataProvider = adapter.DataProvider as Entity;
                if ((dataProvider != null) && dataProvider.HasComponent<SelectedListItemComponent>())
                {
                    dataProvider.RemoveComponent<SelectedListItemComponent>();
                }
            }
        }

        private void Update()
        {
            this.Layout();
        }

        private void UpdateBounds(ListDataProvider provider)
        {
            this.UpdateSize();
        }

        private void UpdateSize()
        {
            <UpdateSize>c__AnonStorey0 storey = new <UpdateSize>c__AnonStorey0 {
                $this = this
            };
            this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.viewport.rect.width - 1f);
            storey.height = 0;
            this.Adapters.ForEach(new Action<GoodsContentAdapter>(storey.<>m__0));
            storey.height -= this.spacing;
            this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float) storey.height);
            if (storey.height < this.rectTransform.anchoredPosition.y)
            {
                this.rectTransform.anchoredPosition = new Vector2(0f, Math.Max((float) 0f, (float) (storey.height - this.viewport.rect.height)));
            }
        }

        private float end =>
            this.rectTransform.anchoredPosition.y + this.viewport.rect.height;

        private float start =>
            this.rectTransform.anchoredPosition.y;

        [CompilerGenerated]
        private sealed class <UpdateSize>c__AnonStorey0
        {
            internal int height;
            internal GoodsDynamicVerticalList $this;

            internal void <>m__0(GoodsDynamicVerticalList.GoodsContentAdapter x)
            {
                this.height += (x.Content.Height + this.$this.spacing) * x.DataProvider.Data.Count;
                this.height += x.DataProvider.CommentCount * 200;
            }
        }

        [Serializable]
        public class ContentAdapter
        {
            public GoodsDynamicVerticalList.ItemContent Content;
            public CommentedListDataProvider DataProvider;
        }

        [Serializable]
        public class GoodsContentAdapter : GoodsDynamicVerticalList.ContentAdapter
        {
            public GoodsDynamicVerticalList.GoodsType Type;
        }

        [SerializeField]
        public enum GoodsType
        {
            XCrystals,
            SpecialOffer
        }

        [Serializable]
        public class ItemContent
        {
            public RectTransform Prefab;
            public int Height;
        }
    }
}

