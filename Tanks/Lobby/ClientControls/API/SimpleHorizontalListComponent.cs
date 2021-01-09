namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class SimpleHorizontalListComponent : MonoBehaviour, IScrollHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, Component, IEventSystemHandler
    {
        [SerializeField]
        private UnityEngine.RectTransform horizontalLayoutGroup;
        [SerializeField]
        private UnityEngine.RectTransform leftButtonPlace;
        [SerializeField]
        private UnityEngine.RectTransform rightButtonPlace;
        [SerializeField]
        private UnityEngine.RectTransform content;
        [SerializeField]
        private UnityEngine.RectTransform scrollRect;
        [SerializeField]
        private ListItem itemPrefab;
        [SerializeField]
        private EntityBehaviour itemContentPrefab;
        [SerializeField]
        private UnityEngine.RectTransform navigationButtonPrefab;
        [SerializeField]
        private float navigationButtonsScrollTime = 0.3f;
        private ItemsMap items = new ItemsMap();
        private ListItem selectedItem;
        private UnityEngine.RectTransform leftButton;
        private UnityEngine.RectTransform rightButton;
        private float spaceBetweenNavigationButtons;
        private float spaceBetweenElements;
        private float position;
        private float velocity;
        private float navigationButtonsScrollVelocity;
        private int targetItemIndex;
        private int previousScrollRectWidth;
        private int calculatedItemWidth;
        private bool animating;
        private bool noScroll;
        private bool needDestroyNavigationButton;
        private float dragDirection;
        private Canvas canvas;
        private bool draging;
        private bool isKeyboardNavigationAllowed = true;
        private SimpleHorizontalListItemsSorter sorter;
        private bool checkedSorter;
        [CompilerGenerated]
        private static Func<ListItem, Entity> <>f__am$cache0;

        public void AddItem(Entity entity)
        {
            EntityBehaviour behaviour = Instantiate<EntityBehaviour>(this.itemContentPrefab);
            ListItem item = Instantiate<ListItem>(this.itemPrefab);
            item.SetContent((UnityEngine.RectTransform) behaviour.transform);
            item.transform.SetParent(this.content, false);
            item.Data = entity;
            this.items.Add(item);
            behaviour.BuildEntity(entity);
            if (this.Sorter == null)
            {
                item.transform.SetAsLastSibling();
            }
            else
            {
                this.Sorter.Sort(this.items);
                int num = 0;
                foreach (ListItem item2 in this.items)
                {
                    item2.transform.SetSiblingIndex(num++);
                }
            }
            this.Layout();
        }

        private void ApplyPosition()
        {
            this.position = this.ClampPosition(this.position);
            this.content.anchoredPosition = new Vector2(this.position, 0f);
            this.UpdateNavigationButtonsVisibility();
        }

        private void Awake()
        {
            this.spaceBetweenNavigationButtons = this.horizontalLayoutGroup.GetComponent<SimpleHorizontalLayoutGroup>().spacing;
            this.previousScrollRectWidth = (int) this.scrollRect.rect.width;
            float width = this.navigationButtonPrefab.rect.width;
            this.leftButtonPlace.GetComponent<SimpleLayoutElement>().maxWidth = width;
            this.rightButtonPlace.GetComponent<SimpleLayoutElement>().maxWidth = width;
        }

        private int CalculateItemsWidth(int count, float itemWidth) => 
            (int) ((itemWidth * count) + (this.spaceBetweenElements * (count - 1)));

        private int CalculateItemWidth()
        {
            float minWidth = this.itemPrefab.GetComponent<LayoutElement>().minWidth;
            int a = 1;
            while (true)
            {
                Rect rect = this.scrollRect.rect;
                if (this.CalculateItemsWidth(a + 1, minWidth) >= rect.width)
                {
                    a = Mathf.Min(a, this.items.Count);
                    int num3 = this.CalculateItemsWidth(a, minWidth);
                    return (((int) minWidth) + ((int) ((this.scrollRect.rect.width - num3) / ((float) a))));
                }
                a++;
            }
        }

        private float ClampPosition(float pos)
        {
            if (pos >= 0f)
            {
                return 0f;
            }
            float minPosition = this.GetMinPosition();
            return ((pos >= minPosition) ? pos : minPosition);
        }

        private void ClearHighlight()
        {
            this.animating = false;
            this.content.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        public void ClearItems(bool immediate = false)
        {
            foreach (ListItem item in this.items)
            {
                EntityBehaviour.CleanUp(item.gameObject);
                if (immediate)
                {
                    DestroyImmediate(item.gameObject);
                    continue;
                }
                Destroy(item.gameObject);
            }
            this.items.Clear();
            this.selectedItem = null;
            this.position = 0f;
            this.targetItemIndex = 0;
            this.velocity = 0f;
            this.animating = false;
            this.ApplyPosition();
        }

        public bool Contains(Entity entity) => 
            this.items.Contains(entity);

        private int CorrectIndex(int index)
        {
            if (index < 0)
            {
                return 0;
            }
            int maxTargetIndex = this.GetMaxTargetIndex();
            return ((index <= maxTargetIndex) ? index : maxTargetIndex);
        }

        private void CreateNavigationButtons()
        {
            if (this.leftButton == null)
            {
                this.leftButton = Instantiate<UnityEngine.RectTransform>(this.navigationButtonPrefab);
                this.InitNavigationButton(this.leftButton, this.leftButtonPlace.transform, -1, new UnityAction(this.OnLeftButtonClick));
            }
            if (this.rightButton == null)
            {
                this.rightButton = Instantiate<UnityEngine.RectTransform>(this.navigationButtonPrefab);
                this.InitNavigationButton(this.rightButton, this.rightButtonPlace.transform, 1, new UnityAction(this.OnRightButtonClick));
            }
        }

        private void DestroyNavigationButtons()
        {
            if (this.leftButton != null)
            {
                Destroy(this.leftButton.gameObject);
                this.leftButton = null;
            }
            if (this.rightButton != null)
            {
                Destroy(this.rightButton.gameObject);
                this.rightButton = null;
            }
        }

        private void ExtendItemsWidth(int newItemWidth)
        {
            foreach (ListItem item in this.items)
            {
                item.GetComponent<LayoutElement>().minWidth = newItemWidth;
            }
        }

        public int GetIndex(Entity entity) => 
            this.items[entity].transform.GetSiblingIndex();

        public GameObject GetItem(Entity entity) => 
            this.items[entity].gameObject;

        public ICollection<Entity> GetItems()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = item => (Entity) item.Data;
            }
            return this.items.Select<ListItem, Entity>(<>f__am$cache0).ToList<Entity>();
        }

        private int GetMaxTargetIndex()
        {
            float minPosition = this.GetMinPosition();
            return this.PositionToItemIndex(minPosition);
        }

        private float GetMinPosition() => 
            Mathf.Min((float) 0f, (float) (this.scrollRect.rect.width - this.CalculateItemsWidth(this.items.Count, (float) this.calculatedItemWidth)));

        private void InitNavigationButton(UnityEngine.RectTransform button, Transform parent, int scale, UnityAction clickHandler)
        {
            button.GetComponent<Button>().onClick.AddListener(clickHandler);
            button.SetParent(parent, false);
            button.localScale = new Vector3((float) scale, 1f, 1f);
            button.pivot = new Vector2(0.5f - (scale * 0.5f), 0.5f);
            button.anchorMin = new Vector2(0f, 0.5f);
            button.anchorMax = new Vector2(0f, 0.5f);
        }

        private float ItemIndexToPosition(int itemIndex) => 
            -itemIndex * (this.calculatedItemWidth + this.spaceBetweenElements);

        private void Layout()
        {
            LayoutElement component = this.itemPrefab.GetComponent<LayoutElement>();
            int count = this.items.Count;
            this.calculatedItemWidth = this.CalculateItemWidth();
            float minWidth = component.minWidth;
            this.spaceBetweenElements = this.content.GetComponent<HorizontalLayoutGroup>().spacing;
            if (((int) ((minWidth * count) + (this.spaceBetweenElements * (count - 1)))) > this.RectTransform.rect.width)
            {
                this.LayoutWithNavigationButtons();
            }
            else
            {
                this.LayoutWithoutNavigationButtons();
                if (component.preferredWidth > 0f)
                {
                    this.calculatedItemWidth = Mathf.Min(this.calculatedItemWidth, (int) component.preferredWidth);
                }
            }
            this.ExtendItemsWidth(this.calculatedItemWidth);
            this.navigationButtonsScrollVelocity = (this.calculatedItemWidth + this.spaceBetweenElements) / this.navigationButtonsScrollTime;
        }

        private void LayoutWithNavigationButtons()
        {
            this.noScroll = false;
            this.horizontalLayoutGroup.GetComponent<SimpleHorizontalLayoutGroup>().spacing = this.spaceBetweenNavigationButtons;
            this.leftButtonPlace.GetComponent<SimpleLayoutElement>().flexibleWidth = 1f;
            this.rightButtonPlace.GetComponent<SimpleLayoutElement>().flexibleWidth = 1f;
            this.CreateNavigationButtons();
        }

        private void LayoutWithoutNavigationButtons()
        {
            this.noScroll = true;
            this.needDestroyNavigationButton = true;
            this.ClearHighlight();
            this.horizontalLayoutGroup.GetComponent<SimpleHorizontalLayoutGroup>().spacing = 0f;
            this.leftButtonPlace.GetComponent<SimpleLayoutElement>().flexibleWidth = 0f;
            this.rightButtonPlace.GetComponent<SimpleLayoutElement>().flexibleWidth = 0f;
            this.targetItemIndex = 0;
            this.position = 0f;
            this.ApplyPosition();
        }

        public void MoveToItem(Entity entity)
        {
            this.targetItemIndex = this.items[entity].transform.GetSiblingIndex();
            this.velocity = this.navigationButtonsScrollVelocity;
            this.SetPositionToTarget();
            this.ClearHighlight();
        }

        public void MoveToItem(GameObject obj)
        {
            Entity entity = obj.GetComponentInParent<EntityBehaviour>().Entity;
            if (this.items.Contains(entity))
            {
                this.MoveToItem(entity);
            }
        }

        public void MoveToItemAnimated(object entity)
        {
            this.targetItemIndex = this.items[entity].transform.GetSiblingIndex();
            this.velocity = this.navigationButtonsScrollVelocity;
        }

        private void OnBaseElementSizeChanged()
        {
            this.Layout();
            this.ToNearestItem();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!this.noScroll)
            {
                this.draging = true;
                this.dragDirection = 0f;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!this.noScroll)
            {
                if (this.canvas == null)
                {
                    this.canvas = base.GetComponentInParent<BaseElementCanvasScaler>().GetComponent<Canvas>();
                }
                float num = eventData.delta.x / this.canvas.scaleFactor;
                this.position += num;
                this.dragDirection += num;
                this.ApplyPosition();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!this.noScroll)
            {
                this.draging = false;
                this.ToNearestItem();
                this.ClearHighlight();
            }
        }

        private void OnGUI()
        {
            if (this.needDestroyNavigationButton)
            {
                this.needDestroyNavigationButton = false;
                this.DestroyNavigationButtons();
            }
            if (!this.previousScrollRectWidth.Equals((int) this.scrollRect.rect.width))
            {
                this.previousScrollRectWidth = (int) this.scrollRect.rect.width;
                this.Layout();
                this.SetPositionToTarget();
            }
            if (this.isKeyboardNavigationAllowed && (!this.noScroll && (Event.current.type == EventType.KeyDown)))
            {
                float horizontal = InputMapping.Horizontal;
                int index = -1;
                if (horizontal > 0f)
                {
                    index = this.selectedItem.transform.GetSiblingIndex() + 1;
                }
                else if (horizontal < 0f)
                {
                    index = this.selectedItem.transform.GetSiblingIndex() - 1;
                }
                if ((index >= 0) && (index < this.content.childCount))
                {
                    Transform child = this.content.GetChild(index);
                    child.GetComponent<ListItem>().Select(true);
                    this.MoveToItemAnimated(child.GetComponent<ListItem>().Data);
                }
            }
        }

        private void OnItemSelect(ListItem item)
        {
            if (this.selectedItem != null)
            {
                this.selectedItem.PlayDeselectionAnimation();
            }
            this.selectedItem = item;
            EngineService.Engine.ScheduleEvent<ListItemSelectedEvent>((Entity) this.selectedItem.Data);
        }

        private void OnLeftButtonClick()
        {
            if (this.targetItemIndex > 0)
            {
                if (this.targetItemIndex > this.GetMaxTargetIndex())
                {
                    this.targetItemIndex = this.GetMaxTargetIndex();
                }
                this.targetItemIndex--;
                this.velocity = this.navigationButtonsScrollVelocity;
            }
        }

        private void OnRightButtonClick()
        {
            if (this.targetItemIndex < this.GetMaxTargetIndex())
            {
                this.targetItemIndex++;
                this.velocity = this.navigationButtonsScrollVelocity;
            }
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (!this.noScroll && !this.draging)
            {
                if (eventData.scrollDelta.y > 0f)
                {
                    this.OnLeftButtonClick();
                }
                else
                {
                    this.OnRightButtonClick();
                }
            }
        }

        private int PositionToItemIndex(float pos) => 
            Mathf.RoundToInt(-pos / (this.calculatedItemWidth + this.spaceBetweenElements));

        public void RemoveItem(Entity entity)
        {
            if (!this.items.Contains(entity))
            {
                throw new ItemNotExistsException(entity);
            }
            ListItem item = this.items[entity];
            if (this.selectedItem == item)
            {
                this.selectedItem = null;
            }
            this.items.Remove(entity);
            EntityBehaviour.CleanUp(item.gameObject);
            DestroyImmediate(item.gameObject);
            this.Layout();
        }

        public void Select(Entity entity)
        {
            if (!this.items.Contains(entity))
            {
                throw new ItemNotExistsException(entity);
            }
            this.OnItemSelect(this.items[entity]);
            this.items[entity].Select(true);
        }

        public void SetIndex(Entity entity, int index)
        {
            this.items[entity].transform.SetSiblingIndex(index);
        }

        private void SetPositionToTarget()
        {
            this.position = this.ItemIndexToPosition(this.CorrectIndex(this.targetItemIndex));
            this.ApplyPosition();
        }

        private void Start()
        {
            this.Layout();
        }

        private void ToNearestItem()
        {
            float num = this.position % (this.calculatedItemWidth + this.spaceBetweenElements);
            this.targetItemIndex = (this.dragDirection >= 0f) ? this.PositionToItemIndex(this.position - num) : this.PositionToItemIndex(this.position - ((this.calculatedItemWidth + this.spaceBetweenElements) + num));
            this.velocity = this.navigationButtonsScrollVelocity;
            this.ApplyPosition();
        }

        private void Update()
        {
            if (!this.draging)
            {
                int itemIndex = this.CorrectIndex(this.targetItemIndex);
                float num2 = this.ItemIndexToPosition(itemIndex);
                if (this.position != num2)
                {
                    if (!this.animating)
                    {
                        this.animating = true;
                        this.content.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    }
                    if (this.position < num2)
                    {
                        this.position += this.velocity * Time.deltaTime;
                        if (this.position > num2)
                        {
                            this.position = num2;
                            this.ClearHighlight();
                        }
                        this.ApplyPosition();
                    }
                    else
                    {
                        this.position -= this.velocity * Time.deltaTime;
                        if (this.position < num2)
                        {
                            this.position = num2;
                            this.ClearHighlight();
                        }
                        this.ApplyPosition();
                    }
                }
            }
        }

        private void UpdateNavigationButtonsVisibility()
        {
            if (this.leftButton != null)
            {
                this.leftButton.gameObject.SetActive(this.position < 0f);
            }
            if (this.rightButton != null)
            {
                float num = this.scrollRect.rect.width - this.CalculateItemsWidth(this.items.Count, (float) this.calculatedItemWidth);
                this.rightButton.gameObject.SetActive(this.position > num);
            }
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        private UnityEngine.RectTransform RectTransform =>
            (UnityEngine.RectTransform) base.transform;

        public SimpleHorizontalListItemsSorter Sorter
        {
            get
            {
                if ((this.sorter == null) && !this.checkedSorter)
                {
                    this.sorter = base.GetComponent<SimpleHorizontalListItemsSorter>();
                    this.checkedSorter = true;
                }
                return this.sorter;
            }
        }

        public bool IsKeyboardNavigationAllowed
        {
            get => 
                this.isKeyboardNavigationAllowed;
            set => 
                this.isKeyboardNavigationAllowed = value;
        }

        public int Count =>
            this.items.Count;
    }
}

