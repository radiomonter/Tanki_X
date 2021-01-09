namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class SlotInteractive : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IEventSystemHandler
    {
        public Image selectionBorder;
        public Image border;
        public Color highlightedColor;
        public Color pressedColor;
        private Color colorMultiplier;
        private bool selectable;
        public ModuleItem moduleItem;
        public Action onClick;
        public Action onDoubleClick;
        private bool selected;

        public void Deselect()
        {
            this.selected = false;
            this.UpdateView();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right)
            {
                if (this.selectable && ((eventData.clickCount == 1) && (this.onClick != null)))
                {
                    this.onClick();
                }
                if ((eventData.clickCount > 1) && (this.onDoubleClick != null))
                {
                    this.onDoubleClick();
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if ((eventData.button != PointerEventData.InputButton.Right) && this.selectable)
            {
                this.SetPressedColor();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (this.selectable)
            {
                this.border.gameObject.SetActive(false);
                if (!this.selected)
                {
                    this.selectionBorder.color = this.highlightedColor * this.colorMultiplier;
                    this.selectionBorder.gameObject.SetActive(true);
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (this.selectable)
            {
                this.border.gameObject.SetActive((this.moduleItem.UserItem != null) && !this.selected);
                this.selectionBorder.gameObject.SetActive(this.selected);
            }
        }

        public void Select()
        {
            this.selected = true;
            this.UpdateView();
        }

        private void SetPressedColor()
        {
            this.selectionBorder.color = this.pressedColor * this.colorMultiplier;
        }

        private void UpdateView()
        {
            this.selectable = (this.moduleItem.UserItem == null) || this.moduleItem.ImproveAvailable();
            if (!this.selectable)
            {
                this.selectionBorder.gameObject.SetActive(false);
                this.border.gameObject.SetActive(true);
            }
            else if (!this.selected)
            {
                this.selectionBorder.gameObject.SetActive(false);
                this.border.gameObject.SetActive(this.moduleItem.IsMounted);
            }
            else
            {
                this.selectionBorder.gameObject.SetActive(true);
                this.border.gameObject.SetActive(false);
                this.SetPressedColor();
            }
            this.border.color = Color.white * this.colorMultiplier;
        }

        public void UpdateView(Color colorMultiplier)
        {
            this.colorMultiplier = colorMultiplier;
            this.UpdateView();
        }
    }
}

