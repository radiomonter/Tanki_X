namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class DropDownListComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
    {
        public OnDropDownListItemSelected onDropDownListItemSelected;
        [SerializeField]
        protected TextMeshProUGUI listTitle;
        [SerializeField]
        protected DefaultListDataProvider dataProvider;
        [SerializeField]
        private float maxHeight = 210f;
        private RectTransform scrollRectContent;
        private RectTransform listRect;
        private bool isOpen;
        private bool pointerOver;
        private bool pointerOverContent;

        private void Awake()
        {
            ScrollRect componentInChildren = base.GetComponentInChildren<ScrollRect>();
            this.scrollRectContent = componentInChildren.content;
            this.listRect = componentInChildren.transform.parent.GetComponent<RectTransform>();
            base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.ClickAction));
            this.IsOpen = false;
        }

        public void ClickAction()
        {
            this.IsOpen = !this.IsOpen;
        }

        protected virtual void OnItemSelect(ListItem item)
        {
            this.IsOpen = false;
            if (this.onDropDownListItemSelected != null)
            {
                this.onDropDownListItemSelected(item);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.pointerOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.pointerOver = false;
        }

        protected virtual void PointerOverContentItem(ListItem item)
        {
            this.pointerOverContent = true;
        }

        private void Update()
        {
            if (this.IsOpen)
            {
                float y = Mathf.Min(this.maxHeight, this.scrollRectContent.rect.height);
                if (this.listRect.sizeDelta.y != y)
                {
                    this.listRect.sizeDelta = new Vector2(this.listRect.sizeDelta.x, y);
                    this.scrollRectContent.anchoredPosition = Vector2.zero;
                    this.scrollRectContent.GetComponentInChildren<DynamicVerticalList>().ScrollToSelection();
                }
                if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && (!this.pointerOverContent && !this.pointerOver))
                {
                    this.IsOpen = false;
                }
            }
            this.pointerOverContent = false;
        }

        public object Selected
        {
            get => 
                this.dataProvider.Selected;
            set
            {
                this.dataProvider.Selected = value;
                this.listTitle.text = this.Selected.ToString();
            }
        }

        public int SelectionIndex
        {
            get => 
                this.dataProvider.Data.IndexOf(this.Selected);
            set => 
                this.Selected = this.dataProvider.Data[value];
        }

        protected bool IsOpen
        {
            get => 
                this.isOpen;
            set
            {
                this.isOpen = value;
                CanvasGroup component = this.listRect.GetComponent<CanvasGroup>();
                component.alpha = !this.isOpen ? 0f : 1f;
                component.interactable = this.isOpen;
                component.blocksRaycasts = this.isOpen;
            }
        }
    }
}

