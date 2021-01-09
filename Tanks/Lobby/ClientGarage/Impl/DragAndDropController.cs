namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl.Tutorial;
    using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
    using tanks.modules.lobby.ClientGarage.Scripts.Impl.NewModules.UI.New.DragAndDrop;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class DragAndDropController : MonoBehaviour, IDropController
    {
        public TankPartCollectionView turretCollectionView;
        public TankPartCollectionView hullCollectionView;
        public CollectionView collectionView;
        public GameObject background;
        private bool changeSize;
        private DropDescriptor delayedDrop;
        public static float OVER_ITEM_Z_OFFSET = -7f;
        public Action<DropDescriptor, DropDescriptor> onDrop;

        public void Awake()
        {
            this.turretCollectionView.activeSlot.GetComponent<DragAndDropCell>().dropController = this;
            this.turretCollectionView.activeSlot2.GetComponent<DragAndDropCell>().dropController = this;
            this.turretCollectionView.passiveSlot.GetComponent<DragAndDropCell>().dropController = this;
            this.hullCollectionView.activeSlot.GetComponent<DragAndDropCell>().dropController = this;
            this.hullCollectionView.activeSlot2.GetComponent<DragAndDropCell>().dropController = this;
            this.hullCollectionView.passiveSlot.GetComponent<DragAndDropCell>().dropController = this;
            foreach (CollectionSlotView view in CollectionView.slots.Values)
            {
                view.GetComponent<DragAndDropCell>().dropController = this;
            }
        }

        private bool CellIsTankSlot(DragAndDropCell cell) => 
            cell.GetComponent<TankSlotView>() != null;

        private void CorrectFinishDrag()
        {
            DragAndDropItem draggedItem = DragAndDropItem.draggedItem;
            if (draggedItem != null)
            {
                draggedItem.OnEndDrag(null);
                this.OnAnyItemDragEnd(draggedItem, null);
            }
        }

        private bool DraggedItemWasntDrop(DragAndDropItem item) => 
            DragAndDropItem.sourceCell.Equals(item.GetComponentInParent<DragAndDropCell>());

        private void HighlightPossibleSlots()
        {
            ModuleItem moduleItem = DragAndDropItem.draggedItem.GetComponent<SlotItemView>().ModuleItem;
            this.hullCollectionView.TurnOffSlots();
            this.turretCollectionView.TurnOffSlots();
            if (moduleItem.TankPartModuleType == TankPartModuleType.WEAPON)
            {
                this.turretCollectionView.TurnOnSlotsByTypeAndHighlightForDrop(moduleItem.ModuleBehaviourType);
            }
            else
            {
                this.hullCollectionView.TurnOnSlotsByTypeAndHighlightForDrop(moduleItem.ModuleBehaviourType);
            }
            foreach (KeyValuePair<ModuleItem, CollectionSlotView> pair in CollectionView.slots)
            {
                CollectionSlotView view = pair.Value;
                if (pair.Key == moduleItem)
                {
                    view.TurnOnRenderAboveAll();
                    continue;
                }
                view.GetComponent<DragAndDropCell>().enabled = false;
            }
        }

        private void MoveDraggingCardInFronfOfAll(float zOffset)
        {
            Vector3 vector = DragAndDropItem.draggedItemContentCopy.GetComponent<RectTransform>().anchoredPosition3D;
            vector.z = zOffset;
            DragAndDropItem.draggedItemContentCopy.GetComponent<RectTransform>().anchoredPosition3D = vector;
            this.TurnOnRenderAboveAll(DragAndDropItem.draggedItemContentCopy);
        }

        private void OnAnyItemDragEnd(DragAndDropItem item, PointerEventData eventData)
        {
            this.background.SetActive(false);
            this.turretCollectionView.CancelHighlightForDrop();
            this.hullCollectionView.CancelHighlightForDrop();
            foreach (KeyValuePair<ModuleItem, CollectionSlotView> pair in CollectionView.slots)
            {
                CollectionSlotView view = pair.Value;
                view.GetComponent<DragAndDropCell>().enabled = true;
                view.TurnOffRenderAboveAll();
            }
        }

        private void OnAnyItemDragStart(DragAndDropItem item, PointerEventData eventData)
        {
            float num = NewModulesScreenUIComponent.OVER_SCREEN_Z_OFFSET;
            if (!ModulesTutorialUtil.TUTORIAL_MODE)
            {
                this.background.SetActive(true);
                this.background.transform.SetAsLastSibling();
                Vector3 vector = this.background.GetComponent<RectTransform>().anchoredPosition3D;
                vector.z = (num * 0.5f) - 0.01f;
                this.background.GetComponent<RectTransform>().anchoredPosition3D = vector;
            }
            this.HighlightPossibleSlots();
            this.MoveDraggingCardInFronfOfAll(num + OVER_ITEM_Z_OFFSET);
            DragAndDropItem.draggedItemContentCopy.transform.GetChild(0).GetComponent<Animator>().SetInteger("colorCode", 1);
            if (DragAndDropItem.sourceCell.GetComponent<SlotView>() is CollectionSlotView)
            {
                this.changeSize = true;
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            this.CorrectFinishDrag();
        }

        public void OnDisable()
        {
            DragAndDropItem.OnItemDragStartEvent -= new DragAndDropItem.DragEvent(this.OnAnyItemDragStart);
            DragAndDropItem.OnItemDragEndEvent -= new DragAndDropItem.DragEvent(this.OnAnyItemDragEnd);
            this.CorrectFinishDrag();
        }

        public void OnDrop(DragAndDropCell cellFrom, DragAndDropCell cellTo, DragAndDropItem item)
        {
            if ((item != null) && (cellFrom != cellTo))
            {
                DropDescriptor descriptor2 = new DropDescriptor {
                    item = item,
                    sourceCell = cellFrom,
                    destinationCell = cellTo
                };
                DropDescriptor descriptor = descriptor2;
                if (!this.CellIsTankSlot(cellTo))
                {
                    descriptor.destinationCell.PlaceItem(descriptor.item);
                    if (this.onDrop != null)
                    {
                        descriptor2 = new DropDescriptor();
                        this.onDrop(descriptor, descriptor2);
                    }
                }
                else if (this.CellIsTankSlot(cellFrom))
                {
                    descriptor2 = new DropDescriptor {
                        destinationCell = descriptor.sourceCell,
                        item = descriptor.destinationCell.GetItem(),
                        sourceCell = descriptor.destinationCell
                    };
                    DropDescriptor descriptor3 = descriptor2;
                    descriptor.destinationCell.PlaceItem(descriptor.item);
                    if (descriptor3.item != null)
                    {
                        descriptor.sourceCell.PlaceItem(descriptor3.item);
                    }
                    if (this.onDrop != null)
                    {
                        this.onDrop(descriptor, descriptor3);
                    }
                }
                else
                {
                    DragAndDropItem item2 = descriptor.destinationCell.GetItem();
                    DragAndDropCell component = null;
                    if (item2 != null)
                    {
                        ModuleItem moduleItem = item2.GetComponent<SlotItemView>().ModuleItem;
                        component = CollectionView.slots[moduleItem].GetComponent<DragAndDropCell>();
                    }
                    descriptor2 = new DropDescriptor {
                        destinationCell = component,
                        item = item2,
                        sourceCell = descriptor.destinationCell
                    };
                    DropDescriptor descriptor4 = descriptor2;
                    descriptor.destinationCell.PlaceItem(descriptor.item);
                    if (descriptor4.item != null)
                    {
                        descriptor4.destinationCell.PlaceItem(descriptor4.item);
                    }
                    if (this.onDrop != null)
                    {
                        this.onDrop(descriptor, descriptor4);
                    }
                }
            }
        }

        public void OnEnable()
        {
            this.background.SetActive(false);
            DragAndDropItem.OnItemDragStartEvent += new DragAndDropItem.DragEvent(this.OnAnyItemDragStart);
            DragAndDropItem.OnItemDragEndEvent += new DragAndDropItem.DragEvent(this.OnAnyItemDragEnd);
        }

        public void TurnOnRenderAboveAll(GameObject gameObject)
        {
            Canvas canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.overrideSorting = true;
            canvas.sortingOrder = 30;
            gameObject.AddComponent<GraphicRaycaster>();
            CanvasGroup group = gameObject.GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
            group.blocksRaycasts = true;
            group.ignoreParentGroups = true;
            group.interactable = false;
        }

        private void Update()
        {
            if (this.changeSize)
            {
                this.changeSize = false;
                DragAndDropItem.draggedItemContentCopy.GetComponent<Animator>().SetBool("GrowUp", true);
            }
            if (this.delayedDrop.item != null)
            {
                this.OnDrop(this.delayedDrop.sourceCell, this.delayedDrop.destinationCell, this.delayedDrop.item);
                this.delayedDrop.sourceCell = null;
                this.delayedDrop.destinationCell = null;
                this.delayedDrop.item = null;
            }
        }
    }
}

