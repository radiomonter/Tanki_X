namespace Tanks.Lobby.ClientControls.API
{
    using log4net;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class LazyListComponent : UIBehaviour, Component, ILazyList, IScrollHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IEventSystemHandler
    {
        private const float EPSILON = 0.01f;
        [SerializeField]
        private ListItem itemPrefab;
        [SerializeField]
        private EntityBehaviour entityBehaviour;
        [SerializeField]
        private float itemMinSize = 100f;
        [SerializeField]
        private float spacing;
        [SerializeField]
        private bool vertical;
        [SerializeField]
        private bool noScroll;
        [SerializeField]
        private float itemScrollTime = 0.2f;
        private ILog log;
        private RectTransform rectTransform;
        private Canvas canvas;
        private float position;
        private float targetPosition;
        private int targetItemIndex;
        private int itemsCount;
        private IndexRange visibleItemsRange;
        private IndexRange screenRange;
        private float pageSize;
        private float prevPageSize;
        private int itemsPerPage;
        private float itemSize;
        private float allContentSize;
        private bool dragging;
        private bool dragDirectionPositive;
        private bool quiting;
        private bool forceNextUpdate = true;
        private int selectedItemIndex;
        private Entity selectedEntity;
        private bool inSelectMode;

        private bool AtTarget() => 
            this.position == this.targetPosition;

        protected override void Awake()
        {
            this.log = LoggerProvider.GetLogger(this);
            this.rectTransform = base.GetComponent<RectTransform>();
            this.canvas = base.GetComponentInParent<Canvas>();
        }

        private IndexRange CalculateScreenRange() => 
            IndexRange.CreateFromBeginAndEnd(this.PositionToItemIndexUnclamped(this.position + 0.01f), this.PositionToItemIndexUnclamped((this.position + this.pageSize) - 0.01f));

        private IndexRange CalculateVisibleItemsRange()
        {
            if (this.itemsPerPage == 0)
            {
                return new IndexRange();
            }
            int position = this.PositionToItemIndex(this.position + 0.01f);
            int endPosition = this.PositionToItemIndex((this.position + this.pageSize) - 0.01f);
            if ((position >= 0) && (endPosition == -1))
            {
                endPosition = this.itemsCount - 1;
            }
            return IndexRange.CreateFromBeginAndEnd(position, endPosition);
        }

        private int ChildIndexToItemIndex(int childIndex) => 
            childIndex + this.visibleItemsRange.Position;

        private void ClampPosition()
        {
            if (this.targetPosition > (this.allContentSize - this.pageSize))
            {
                this.targetPosition = this.allContentSize - this.pageSize;
                if (this.position > this.targetPosition)
                {
                    this.position = this.targetPosition;
                }
            }
            if (this.targetPosition < 0f)
            {
                this.targetPosition = 0f;
                if (this.position < 0f)
                {
                    this.position = 0f;
                }
            }
        }

        private void ClampTargetItemIndex()
        {
            if ((this.targetItemIndex + this.itemsPerPage) > this.itemsCount)
            {
                this.targetItemIndex = this.itemsCount - this.itemsPerPage;
            }
            if (this.targetItemIndex < 0)
            {
                this.targetItemIndex = 0;
            }
        }

        public void ClearItems()
        {
            this.UnselectItemContent();
            IndexRange newVisibleItemsRange = new IndexRange();
            this.UpdateVisibility(newVisibleItemsRange, this.CalculateScreenRange(), false);
            IndexRange range2 = new IndexRange();
            this.screenRange = range2;
            this.itemsCount = 0;
            this.position = 0f;
            this.targetPosition = 0f;
            this.targetItemIndex = 0;
            this.dragging = false;
            this.selectedItemIndex = 0;
        }

        private ListItem CreateItem()
        {
            ListItem item = Instantiate<ListItem>(this.itemPrefab);
            item.transform.SetParent(this.rectTransform, false);
            return item;
        }

        [DebuggerHidden]
        private IEnumerator DelaySelection(int dir) => 
            new <DelaySelection>c__Iterator0 { 
                dir = dir,
                $this = this
            };

        private static float GetAllContentSize(int itemsCount, float itemSize, float spacing)
        {
            float num = itemsCount * (itemSize + spacing);
            if (itemsCount > 1)
            {
                num -= spacing;
            }
            return num;
        }

        private int GetAxis() => 
            !this.vertical ? 0 : 1;

        private static Entity GetEntity(Transform content)
        {
            if (content != null)
            {
                EntityBehaviour component = content.GetComponent<EntityBehaviour>();
                if (component != null)
                {
                    return component.Entity;
                }
            }
            return null;
        }

        public RectTransform GetItem(int itemIndex)
        {
            int index = this.ItemIndexToChildIndex(itemIndex);
            return ((index == -1) ? null : ((RectTransform) this.rectTransform.GetChild(index)));
        }

        private float GetItemCenterPosition(int itemIndex) => 
            this.GetItemStartPosition(itemIndex) + (this.itemSize / 2f);

        public RectTransform GetItemContent(int itemIndex)
        {
            RectTransform item = this.GetItem(itemIndex);
            return item?.GetComponent<ListItem>().GetContent();
        }

        private static float GetItemSize(float pageSize, int itemsPerPage, float spacing) => 
            (itemsPerPage <= 0) ? 0f : ((pageSize - (spacing * (itemsPerPage - 1))) / ((float) itemsPerPage));

        private static int GetItemsPerPage(float itemsMinSize, float spacing, float pageSize)
        {
            if (itemsMinSize > 0f)
            {
                float num = pageSize;
                if (itemsMinSize <= num)
                {
                    return (1 + Mathf.FloorToInt((num - itemsMinSize) / (itemsMinSize + spacing)));
                }
            }
            return 0;
        }

        private float GetItemStartPosition(int itemIndex) => 
            itemIndex * (this.itemSize + this.spacing);

        private float GetSize()
        {
            float num = this.rectTransform.rect.size[this.GetAxis()];
            return ((num >= 0f) ? num : 0f);
        }

        private bool IsItemVisible(int itemIndex) => 
            this.visibleItemsRange.Contains(itemIndex);

        private bool IsSizeChanged() => 
            this.GetSize() != this.prevPageSize;

        private int ItemIndexToChildIndex(int itemIndex) => 
            this.IsItemVisible(itemIndex) ? (itemIndex - this.visibleItemsRange.Position) : -1;

        private void Layout()
        {
            bool sizeChanged = this.IsSizeChanged();
            this.UpdatePageSizes();
            this.UpdatePagePosition(sizeChanged);
            this.UpdateVisibility(this.CalculateVisibleItemsRange(), this.CalculateScreenRange(), true);
            this.UpdateItemsPositionsAndSizes();
            this.SendScrollLimitEvent();
        }

        private void OnApplicationQuit()
        {
            this.quiting = true;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!this.noScroll)
            {
                this.targetPosition = this.position;
                this.dragging = true;
            }
        }

        protected override void OnDisable()
        {
            if (!this.quiting)
            {
                this.ClearItems();
                this.forceNextUpdate = true;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!this.noScroll)
            {
                float num = eventData.delta[this.GetAxis()] / this.canvas.scaleFactor;
                this.position -= num;
                this.dragDirectionPositive = num >= 0f;
                this.targetPosition = this.position;
                this.ClampPosition();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!this.noScroll)
            {
                this.dragging = false;
                this.targetItemIndex = this.PositionToItemIndex(this.position);
                if (!this.dragDirectionPositive)
                {
                    this.targetItemIndex++;
                }
                this.targetPosition = this.GetItemStartPosition(this.targetItemIndex);
                this.ClampPosition();
            }
        }

        private void OnGUI()
        {
            if ((Event.current.type == EventType.KeyDown) && !this.inSelectMode)
            {
                float horizontal = InputMapping.Horizontal;
                if (horizontal > 0f)
                {
                    base.StartCoroutine(this.DelaySelection(1));
                }
                else if (horizontal < 0f)
                {
                    base.StartCoroutine(this.DelaySelection(-1));
                }
            }
        }

        private void OnItemInvisible(int childIndex, bool canDestroyImmediate)
        {
            this.log.DebugFormat("OnItemInvisible childIndex={0}", childIndex);
            Transform child = this.rectTransform.GetChild(childIndex);
            if (canDestroyImmediate)
            {
                DestroyImmediate(child.gameObject);
            }
            else
            {
                Destroy(child.gameObject);
            }
        }

        private void OnItemSelect(ListItem listItem)
        {
            this.SelectItem(listItem);
            this.SelectItemContent();
        }

        private void OnItemVisible(int childIndex)
        {
            this.log.DebugFormat("OnItemVisible childIndex={0}", childIndex);
            this.CreateItem().transform.SetSiblingIndex(childIndex);
        }

        public void OnRemoveItemContent(int itemIndex)
        {
            this.log.DebugFormat("OnRemoveItemContent itemIndex={0}", itemIndex);
            if (itemIndex == this.selectedItemIndex)
            {
                this.UnselectItemContent();
            }
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (!this.noScroll)
            {
                if (eventData.scrollDelta.y > 0f)
                {
                    this.Scroll(-1);
                }
                else
                {
                    this.Scroll(1);
                }
            }
        }

        private int PositionToItemIndex(float pos)
        {
            if (this.itemsCount == 0)
            {
                return -1;
            }
            float num = this.itemSize + this.spacing;
            int num2 = -1;
            if (pos >= num)
            {
                num2 = 1 + Mathf.FloorToInt((pos - num) / num);
            }
            else if (pos > 0f)
            {
                num2 = 0;
            }
            if (num2 > (this.itemsCount - 1))
            {
                num2 = -1;
            }
            return num2;
        }

        private int PositionToItemIndexUnclamped(float pos)
        {
            if (this.itemsPerPage == 0)
            {
                return 0;
            }
            float num = this.itemSize + this.spacing;
            return Mathf.FloorToInt(pos / num);
        }

        public void Scroll(int deltaItems)
        {
            this.targetItemIndex += deltaItems;
            this.ClampTargetItemIndex();
            this.targetPosition = this.GetItemStartPosition(this.targetItemIndex);
            this.ClampPosition();
        }

        private void SelectItem(ListItem listItem)
        {
            int selectedItemIndex = this.selectedItemIndex;
            this.selectedItemIndex = this.ChildIndexToItemIndex(listItem.transform.GetSiblingIndex());
            this.log.DebugFormat("SelectItem prevSelectedItemIndex={0} selectedItemIndex={1}", selectedItemIndex, this.selectedItemIndex);
            RectTransform item = this.GetItem(selectedItemIndex);
            if (item != null)
            {
                item.GetComponent<ListItem>().PlayDeselectionAnimation();
            }
            RectTransform transform2 = this.GetItem(this.selectedItemIndex);
            if (transform2 != null)
            {
                transform2.GetComponent<ListItem>().PlaySelectionAnimation();
            }
        }

        private void SelectItemContent()
        {
            this.log.DebugFormat("SelectItemContent OUTER selectedItemIndex={0}", this.selectedItemIndex);
            Entity objA = GetEntity(this.GetItemContent(this.selectedItemIndex));
            if (!ReferenceEquals(objA, this.selectedEntity))
            {
                this.UnselectItemContent();
                if (objA != null)
                {
                    this.selectedEntity = objA;
                    this.log.DebugFormat("SelectItemContent INNER selectedEntity={0}", objA);
                    objA.AddComponent<SelectedListItemComponent>();
                }
            }
        }

        private void SendScrollLimitEvent()
        {
            bool flag = this.position <= 0.01f;
            bool flag2 = this.position >= ((this.allContentSize - this.pageSize) - 0.01f);
            if ((flag != this.AtLimitLow) || (flag2 != this.AtLimitHigh))
            {
                this.AtLimitLow = flag;
                this.AtLimitHigh = flag2;
                EngineService.Engine.ScheduleEvent<ScrollLimitEvent>(this.entityBehaviour.Entity);
            }
        }

        public void SetItemContent(int itemIndex, RectTransform content)
        {
            this.log.DebugFormat("SetItemContent itemIndex={0}", itemIndex);
            RectTransform item = this.GetItem(itemIndex);
            if (item != null)
            {
                item.GetComponent<ListItem>().SetContent(content);
            }
        }

        private void UnselectItemContent()
        {
            this.log.DebugFormat("UnselectItemContent OUTER selectedEntity={0}", this.selectedEntity);
            if (this.selectedEntity != null)
            {
                Entity selectedEntity = this.selectedEntity;
                this.selectedEntity = null;
                this.log.DebugFormat("UnselectItemContent INNER selectedEntity={0}", selectedEntity);
                if (selectedEntity.HasComponent<SelectedListItemComponent>() && !selectedEntity.HasComponent<DeletedEntityComponent>())
                {
                    selectedEntity.RemoveComponent<SelectedListItemComponent>();
                }
            }
        }

        private void Update()
        {
            if (!this.AtTarget() || (this.IsSizeChanged() || (this.dragging || this.forceNextUpdate)))
            {
                this.forceNextUpdate = false;
                this.Layout();
            }
        }

        private void UpdateItemsPositionsAndSizes()
        {
            int position = this.visibleItemsRange.Position;
            for (int i = 0; position <= this.visibleItemsRange.EndPosition; i++)
            {
                RectTransform child = (RectTransform) this.rectTransform.GetChild(i);
                Vector2 sizeDelta = child.sizeDelta;
                sizeDelta[this.GetAxis()] = this.itemSize;
                child.sizeDelta = sizeDelta;
                Vector2 anchoredPosition = child.anchoredPosition;
                anchoredPosition[this.GetAxis()] = this.GetItemCenterPosition(position) - this.position;
                child.anchoredPosition = anchoredPosition;
                position++;
            }
        }

        private void UpdatePagePosition(bool sizeChanged)
        {
            if (sizeChanged)
            {
                this.targetPosition = this.GetItemStartPosition(this.targetItemIndex);
                this.position = this.targetPosition;
                this.ClampPosition();
                this.ClampTargetItemIndex();
            }
            if (this.position != this.targetPosition)
            {
                float num = (this.itemSize + this.spacing) / this.itemScrollTime;
                if (this.position < this.targetPosition)
                {
                    this.position += num * Time.deltaTime;
                    if (this.position > this.targetPosition)
                    {
                        this.position = this.targetPosition;
                    }
                }
                else
                {
                    this.position -= num * Time.deltaTime;
                    if (this.position < this.targetPosition)
                    {
                        this.position = this.targetPosition;
                    }
                }
            }
        }

        private void UpdatePageSizes()
        {
            this.pageSize = this.GetSize();
            this.itemsPerPage = GetItemsPerPage(this.itemMinSize, this.spacing, this.pageSize);
            this.itemSize = GetItemSize(this.pageSize, this.itemsPerPage, this.spacing);
            this.allContentSize = GetAllContentSize(this.itemsCount, this.itemSize, this.spacing);
            this.prevPageSize = this.pageSize;
        }

        public void UpdateSelection(int itemIndex)
        {
            if (itemIndex == this.selectedItemIndex)
            {
                this.SelectItemContent();
            }
        }

        private void UpdateVisibility(IndexRange newVisibleItemsRange, IndexRange newScreenRange, bool canDestroyImmediate)
        {
            IndexRange range;
            IndexRange range2;
            IndexRange range3;
            IndexRange range4;
            this.visibleItemsRange.CalculateDifference(newVisibleItemsRange, out range, out range2, out range3, out range4);
            if (!canDestroyImmediate)
            {
            }
            if (newVisibleItemsRange != this.visibleItemsRange)
            {
                IndexRange visibleItemsRange = this.visibleItemsRange;
                this.visibleItemsRange = newVisibleItemsRange;
                int position = range.Position;
                int childIndex = 0;
                while (true)
                {
                    if (position > range.EndPosition)
                    {
                        int endPosition = range2.EndPosition;
                        int num4 = this.rectTransform.childCount - 1;
                        while (true)
                        {
                            if (endPosition < range2.Position)
                            {
                                int num5 = range3.Position;
                                int num6 = 0;
                                while (true)
                                {
                                    if (num5 > range3.EndPosition)
                                    {
                                        int num7 = range4.Position;
                                        while (true)
                                        {
                                            if (num7 > range4.EndPosition)
                                            {
                                                if (range3.Contains(this.selectedItemIndex) || range4.Contains(this.selectedItemIndex))
                                                {
                                                    this.GetItem(this.selectedItemIndex).GetComponent<ListItem>().PlaySelectionAnimation();
                                                }
                                                EngineService.Engine.ScheduleEvent(new ItemsVisibilityChangedEvent(visibleItemsRange, this.visibleItemsRange), this.entityBehaviour.Entity);
                                                break;
                                            }
                                            this.OnItemVisible(this.rectTransform.childCount);
                                            num7++;
                                        }
                                        break;
                                    }
                                    this.OnItemVisible(num6);
                                    num5++;
                                    num6++;
                                }
                                break;
                            }
                            this.OnItemInvisible(num4, canDestroyImmediate);
                            num4--;
                            endPosition--;
                        }
                        break;
                    }
                    if (!canDestroyImmediate)
                    {
                        childIndex++;
                    }
                    this.OnItemInvisible(childIndex, canDestroyImmediate);
                    position++;
                }
            }
            if (newScreenRange != this.screenRange)
            {
                this.screenRange = newScreenRange;
                EngineService.Engine.ScheduleEvent(new ScreenRangeChangedEvent(this.screenRange), this.entityBehaviour.Entity);
            }
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        public bool AtLimitLow { get; private set; }

        public bool AtLimitHigh { get; private set; }

        public int ItemsCount
        {
            get => 
                this.itemsCount;
            set
            {
                if (this.itemsCount != value)
                {
                    this.itemsCount = value;
                    this.Layout();
                }
            }
        }

        public IndexRange VisibleItemsRange =>
            this.visibleItemsRange;

        [CompilerGenerated]
        private sealed class <DelaySelection>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal int dir;
            internal int <newIndex>__0;
            internal int <index>__0;
            internal float <waitTime>__0;
            internal LazyListComponent $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        if (!this.$this.inSelectMode)
                        {
                            this.<newIndex>__0 = this.$this.selectedItemIndex + this.dir;
                            if ((this.<newIndex>__0 < this.$this.itemsCount) && (this.<newIndex>__0 >= 0))
                            {
                                this.$this.inSelectMode = true;
                                this.<index>__0 = this.$this.ItemIndexToChildIndex(this.<newIndex>__0);
                                this.<waitTime>__0 = 0f;
                                if (this.<index>__0 != -1)
                                {
                                    goto TR_0007;
                                }
                                else
                                {
                                    this.$this.Scroll(this.dir);
                                }
                                goto TR_000B;
                            }
                        }
                        break;

                    case 1:
                        this.<index>__0 = this.$this.ItemIndexToChildIndex(this.<newIndex>__0);
                        this.<waitTime>__0 += Time.deltaTime;
                        if (this.<waitTime>__0 <= 1f)
                        {
                            goto TR_000B;
                        }
                        else
                        {
                            this.$this.inSelectMode = false;
                        }
                        break;

                    case 2:
                        this.<waitTime>__0 += Time.deltaTime;
                        if (this.<waitTime>__0 > 1f)
                        {
                            this.$this.inSelectMode = false;
                            break;
                        }
                        goto TR_0006;

                    default:
                        break;
                }
            TR_0000:
                return false;
            TR_0002:
                return true;
            TR_0006:
                if (this.$this.GetItemContent(this.<newIndex>__0) == null)
                {
                    this.$current = new WaitForEndOfFrame();
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto TR_0002;
                }
                else
                {
                    this.$this.GetItem(this.<newIndex>__0).GetComponent<ListItem>().Select(true);
                    this.$this.inSelectMode = false;
                    this.$PC = -1;
                }
                goto TR_0000;
            TR_0007:
                this.<waitTime>__0 = 0f;
                goto TR_0006;
            TR_000B:
                if (this.<index>__0 == -1)
                {
                    this.$current = new WaitForEndOfFrame();
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto TR_0002;
                }
                goto TR_0007;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

