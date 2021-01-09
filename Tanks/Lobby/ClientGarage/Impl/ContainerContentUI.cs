namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using TMPro;
    using UnityEngine;

    public class ContainerContentUI : MonoBehaviour
    {
        [SerializeField]
        private DefaultListDataProvider dataProvider;
        [SerializeField]
        private Animator contentAnimator;
        [SerializeField]
        private GameObject graffitiRoot;
        [SerializeField]
        private TextMeshProUGUI containerDescription;
        private readonly List<GarageItem> _itemBuffer = new List<GarageItem>(0x40);
        private readonly GarageItemComparer _comparer = new GarageItemComparer();

        public void CheckGraffityVisibility()
        {
            VisualItem selected = this.dataProvider.Selected as VisualItem;
            this.GraffitiRoot.SetActive((selected != null) && (selected.Type == VisualItem.VisualItemType.Graffiti));
        }

        private void OnDisable()
        {
            this.graffitiRoot.SetActive(false);
        }

        public void Set(ContainerBoxItem item, bool selectionIsOn)
        {
            this.Item = item;
            this.dataProvider.ClearItems();
            this._itemBuffer.AddRange(item.Content);
            this._itemBuffer.Sort(this._comparer);
            if (!selectionIsOn)
            {
                this.dataProvider.Init<GarageItem>(this._itemBuffer);
            }
            else
            {
                GarageItem selected = this._itemBuffer.First<GarageItem>();
                this.dataProvider.Init<GarageItem>(this._itemBuffer, selected);
                selected.Select();
            }
            this._itemBuffer.Clear();
            this.containerDescription.text = item.GetLocalizedDescription(item.MarketItem.Id);
            base.GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1f;
        }

        public ContainerBoxItem Item { get; private set; }

        public GameObject GraffitiRoot =>
            this.graffitiRoot;

        private class GarageItemComparer : IComparer<GarageItem>
        {
            public int Compare(GarageItem x, GarageItem y)
            {
                if (x == null)
                {
                    return 1;
                }
                if (y == null)
                {
                    return -1;
                }
                int num3 = ((int) x.Rarity).CompareTo((int) y.Rarity);
                return ((num3 == 0) ? x.CompareByType(y) : num3);
            }
        }
    }
}

