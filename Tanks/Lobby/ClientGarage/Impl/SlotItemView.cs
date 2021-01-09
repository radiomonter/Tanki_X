namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class SlotItemView : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
    {
        public GameObject moduleCard3DPrefab;
        private ModuleCardView moduleCard3D;
        public GameObject itemContent;
        public Action<SlotItemView> onClick;
        public Action<SlotItemView> onDoubleClick;
        public TooltipShowBehaviour tooltip;
        public Animator outline;
        public Color pressedColor;
        public Color highlidhtedColor;
        public Color upgradeColor;
        public float selectionSaturation = 1f;
        public float highlightedSaturation = 0.1f;
        private bool highlightEnable = true;
        private tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items.ModuleItem moduleItem;
        private bool selected;

        public void Awake()
        {
            GameObject obj2 = Instantiate<GameObject>(this.moduleCard3DPrefab);
            obj2.transform.SetParent(this.itemContent.transform, false);
            obj2.transform.position = Vector3.zero;
            this.moduleCard3D = obj2.GetComponent<ModuleCardView>();
        }

        public void Deselect()
        {
            this.selected = false;
            this.UpdateHighlight();
        }

        private void HideHighlight()
        {
            this.outline.SetInteger("colorCode", 0);
        }

        private void HighlightHighlighted()
        {
            this.outline.SetInteger("colorCode", 2);
        }

        private void HighlightPressed()
        {
            this.outline.SetInteger("colorCode", 1);
        }

        private void HighlightUpgrade()
        {
            this.outline.SetInteger("colorCode", 3);
        }

        private void OnEnable()
        {
            if (this.moduleItem != null)
            {
                this.UpdateHighlight();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right)
            {
                if ((this.onClick != null) && (eventData.clickCount == 1))
                {
                    this.onClick(this);
                }
                if ((this.onDoubleClick != null) && (eventData.clickCount > 1))
                {
                    this.onDoubleClick(this);
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if ((eventData.button != PointerEventData.InputButton.Right) && (!this.selected && this.highlightEnable))
            {
                this.HighlightPressed();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!this.selected && this.highlightEnable)
            {
                this.HighlightHighlighted();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!this.selected)
            {
                this.UpdateHighlight();
            }
        }

        public void Select()
        {
            this.selected = true;
            this.UpdateHighlight();
        }

        public void SetScaleToCard3D(float scale)
        {
            this.moduleCard3D.transform.localScale = new Vector3(scale, scale, scale);
        }

        public void UpdateHighlight()
        {
            if (this.selected)
            {
                this.HighlightPressed();
            }
            else if (!this.highlightEnable)
            {
                this.HideHighlight();
            }
            else if (this.moduleItem.ImproveAvailable())
            {
                this.HighlightUpgrade();
            }
            else
            {
                this.HideHighlight();
            }
        }

        public void UpdateView(tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items.ModuleItem moduleItem)
        {
            this.moduleItem = moduleItem;
            this.moduleCard3D.UpdateView(moduleItem.MarketItem.Id, -1L, true, true);
            this.UpdateHighlight();
            this.tooltip.SetCustomContentData(moduleItem);
        }

        public bool HighlightEnable
        {
            get => 
                this.highlightEnable;
            set
            {
                this.highlightEnable = value;
                this.UpdateHighlight();
            }
        }

        public tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items.ModuleItem ModuleItem =>
            this.moduleItem;
    }
}

