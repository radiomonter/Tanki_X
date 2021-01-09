namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;

    public class Carousel : ECSBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler, IEventSystemHandler
    {
        private static bool axisBlockedAtCurrentTick;
        [SerializeField]
        private RectTransform content;
        [SerializeField]
        private float scrollThreshold;
        [SerializeField]
        private GarageItemUI itemPrefab;
        private bool drag;
        private List<GarageItemUI> items = new List<GarageItemUI>();
        private float targetPosition;
        private float startPosition;
        [SerializeField]
        private int selectedItem;
        private int prevSelectedItem = -1;
        [SerializeField]
        private float scrollSpeed = 30f;
        [SerializeField]
        private float fitDuration;
        private float elapsedTime;
        private float inputElapsedTime;
        private int itemsCount;
        [SerializeField]
        private float inputThreshold;
        public UnityAction<GarageItemUI> onItemSelected;
        private float lastScrollTime;
        private float scrollDelta;

        private void AddItem()
        {
            GarageItemUI item = Instantiate<GarageItemUI>(this.itemPrefab);
            item.transform.SetParent(this.content, false);
            this.items.Add(item);
        }

        public void AddItems<T>(List<T> newItems) where T: GarageItem
        {
            this.selectedItem = -1;
            this.prevSelectedItem = -1;
            if (this.items.Count > newItems.Count)
            {
                for (int j = newItems.Count; j < this.items.Count; j++)
                {
                    this.items[j].gameObject.SetActive(false);
                }
            }
            else if (this.items.Count < newItems.Count)
            {
                for (int j = this.items.Count; j < newItems.Count; j++)
                {
                    this.AddItem();
                }
            }
            for (int i = 0; i < newItems.Count; i++)
            {
                this.items[i].gameObject.SetActive(true);
                this.items[i].Init(newItems[i], this);
            }
            this.itemsCount = newItems.Count;
        }

        public static void BlockAxisAtCurrentTick()
        {
            axisBlockedAtCurrentTick = true;
        }

        private int GetCenterItem()
        {
            float x = this.content.anchoredPosition.x;
            int num2 = -1;
            float positiveInfinity = float.PositiveInfinity;
            for (int i = 0; i < this.itemsCount; i++)
            {
                GarageItemUI mui = this.items[i];
                Vector2 anchoredPosition = mui.RectTransform.anchoredPosition;
                float num5 = Mathf.Abs((float) (anchoredPosition.x + x));
                if (num5 < positiveInfinity)
                {
                    num2 = i;
                    positiveInfinity = num5;
                }
            }
            return num2;
        }

        private void LateUpdate()
        {
            if ((this.items.Count == 0) || (this.selectedItem < 0))
            {
                this.elapsedTime = 0f;
            }
            else
            {
                if (this.selectedItem != this.prevSelectedItem)
                {
                    if (this.prevSelectedItem >= 0)
                    {
                        this.items[this.prevSelectedItem].Deselect();
                    }
                    this.items[this.selectedItem].Select();
                    this.onItemSelected(this.items[this.selectedItem]);
                    this.prevSelectedItem = this.selectedItem;
                }
                if (this.drag)
                {
                    this.elapsedTime = 0f;
                }
                else
                {
                    float axis = Input.GetAxis("Horizontal");
                    CheckForTutorialEvent eventInstance = new CheckForTutorialEvent();
                    base.ScheduleEvent(eventInstance, EngineService.EntityStub);
                    if (axisBlockedAtCurrentTick || ((axis == 0f) || ((this.inputElapsedTime < this.inputThreshold) || eventInstance.TutorialIsActive)))
                    {
                        this.inputElapsedTime += Time.deltaTime;
                    }
                    else
                    {
                        if (axis > 0f)
                        {
                            this.selectedItem = Mathf.Min((int) (this.selectedItem + 1), (int) (this.itemsCount - 1));
                        }
                        else if (axis < 0f)
                        {
                            this.selectedItem = Mathf.Max(this.selectedItem - 1, 0);
                        }
                        this.inputElapsedTime = 0f;
                    }
                    axisBlockedAtCurrentTick = false;
                    this.targetPosition = -this.items[this.selectedItem].RectTransform.anchoredPosition.x;
                    if (Mathf.Approximately(this.content.anchoredPosition.x, this.targetPosition))
                    {
                        this.elapsedTime = 0f;
                    }
                    else
                    {
                        Vector2 anchoredPosition = this.content.anchoredPosition;
                        if (this.elapsedTime == 0f)
                        {
                            this.startPosition = anchoredPosition.x;
                        }
                        anchoredPosition.x = Mathf.MoveTowards(anchoredPosition.x, this.targetPosition, (Time.deltaTime * Mathf.Abs((float) (this.startPosition - this.targetPosition))) / this.fitDuration);
                        this.content.anchoredPosition = anchoredPosition;
                        this.elapsedTime += Time.deltaTime;
                    }
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!TutorialCanvas.Instance.IsShow)
            {
                this.drag = true;
            }
        }

        public unsafe void OnDrag(PointerEventData eventData)
        {
            if (!TutorialCanvas.Instance.IsShow)
            {
                Vector2 anchoredPosition = this.content.anchoredPosition;
                Vector2* vectorPtr1 = &anchoredPosition;
                vectorPtr1->x += eventData.delta.x;
                this.content.anchoredPosition = anchoredPosition;
                int centerItem = this.GetCenterItem();
                if (centerItem >= 0)
                {
                    this.selectedItem = centerItem;
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!TutorialCanvas.Instance.IsShow)
            {
                this.drag = false;
                int centerItem = this.GetCenterItem();
                if (centerItem >= 0)
                {
                    this.selectedItem = centerItem;
                }
            }
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (!TutorialCanvas.Instance.IsShow && ((this.lastScrollTime + this.scrollThreshold) < Time.time))
            {
                this.scrollDelta = eventData.scrollDelta.y;
                this.lastScrollTime = Time.time;
                this.selectedItem = (this.scrollDelta >= 0f) ? Mathf.Max(this.selectedItem - 1, 0) : Mathf.Min((int) (this.selectedItem + 1), (int) (this.itemsCount - 1));
            }
        }

        public void RemoveItem(long marketItemId)
        {
            this.selectedItem = -1;
            this.prevSelectedItem = -1;
            GarageItemUI item = null;
            foreach (GarageItemUI mui2 in this.items)
            {
                if (mui2.Item.MarketItem.Id == marketItemId)
                {
                    item = mui2;
                }
            }
            if (item != null)
            {
                this.items.Remove(item);
                Destroy(item.gameObject);
            }
            this.itemsCount = this.items.Count;
        }

        public void Select(GarageItem item, bool immediately = false)
        {
            if ((this.selectedItem < 0) || !ReferenceEquals(this.items[this.selectedItem].Item, item))
            {
                for (int i = 0; i < this.itemsCount; i++)
                {
                    GarageItemUI mui = this.items[i];
                    if (ReferenceEquals(mui.Item, item))
                    {
                        this.selectedItem = i;
                        if (immediately)
                        {
                            this.targetPosition = -this.items[this.selectedItem].RectTransform.anchoredPosition.x;
                            Vector2 anchoredPosition = this.content.anchoredPosition;
                            anchoredPosition.x = this.targetPosition;
                            this.content.anchoredPosition = anchoredPosition;
                        }
                    }
                }
            }
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        public GarageItemUI Selected =>
            this.items[this.selectedItem];

        public bool IsAnySelected =>
            this.selectedItem >= 0;
    }
}

