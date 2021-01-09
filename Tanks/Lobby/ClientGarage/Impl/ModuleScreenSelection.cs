namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Collections.Generic;
    using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
    using UnityEngine.EventSystems;

    public class ModuleScreenSelection
    {
        public Action<ModuleItem> onSelectionChange;
        private static Dictionary<SlotView, ModuleItem> slotToModuleItem;
        private static readonly List<ModuleItem> registeredSlotItems = new List<ModuleItem>();
        public SlotItemView selectedSlotItem;
        public CollectionSlotView selectedSlot;

        public ModuleScreenSelection()
        {
            DragAndDropItem.OnItemDragStartEvent += new DragAndDropItem.DragEvent(this.OnAnyItemDragStart);
            if (slotToModuleItem != null)
            {
                slotToModuleItem.Clear();
                slotToModuleItem = null;
            }
            registeredSlotItems.Clear();
        }

        public void Clear()
        {
            if (this.selectedSlot != null)
            {
                this.selectedSlot.Deselect();
                this.selectedSlot = null;
                if (this.onSelectionChange != null)
                {
                    this.onSelectionChange(null);
                }
            }
            else if (this.selectedSlotItem != null)
            {
                this.selectedSlotItem.Deselect();
                this.selectedSlotItem = null;
                if (this.onSelectionChange != null)
                {
                    this.onSelectionChange(null);
                }
            }
        }

        public ModuleItem GetSelectedModuleItem() => 
            (this.selectedSlot == null) ? this.selectedSlotItem?.ModuleItem : slotToModuleItem[this.selectedSlot];

        private void OnAnyItemDragStart(DragAndDropItem item, PointerEventData eventData)
        {
            SlotItemView component = item.GetComponent<SlotItemView>();
            this.Select(component);
        }

        private void OnItemClick(SlotItemView slotItem)
        {
            this.Select(slotItem);
        }

        private void OnSlotClick(CollectionSlotView slot)
        {
            this.Select(slot);
        }

        public void Select(CollectionSlotView slot)
        {
            if (this.selectedSlot != slot)
            {
                if (this.selectedSlotItem != null)
                {
                    this.selectedSlotItem.Deselect();
                    this.selectedSlotItem = null;
                }
                if (this.selectedSlot != null)
                {
                    this.selectedSlot.Deselect();
                }
                slot.Select();
                this.selectedSlot = slot;
                if (this.onSelectionChange != null)
                {
                    this.onSelectionChange(slotToModuleItem[this.selectedSlot]);
                }
            }
        }

        public void Select(SlotItemView slotItem)
        {
            if (this.selectedSlotItem != slotItem)
            {
                if (this.selectedSlot != null)
                {
                    this.selectedSlot.Deselect();
                    this.selectedSlot = null;
                }
                if (this.selectedSlotItem != null)
                {
                    this.selectedSlotItem.Deselect();
                }
                slotItem.Select();
                this.selectedSlotItem = slotItem;
                if (this.onSelectionChange != null)
                {
                    this.onSelectionChange(slotItem.ModuleItem);
                }
            }
        }

        public void Update(Dictionary<ModuleItem, CollectionSlotView> collectionViewSlots, Dictionary<ModuleItem, SlotItemView> slotItems)
        {
            if (slotToModuleItem == null)
            {
                slotToModuleItem = new Dictionary<SlotView, ModuleItem>();
                foreach (KeyValuePair<ModuleItem, CollectionSlotView> pair in collectionViewSlots)
                {
                    CollectionSlotView key = pair.Value;
                    key.onClick += new Action<CollectionSlotView>(this.OnSlotClick);
                    slotToModuleItem.Add(key, pair.Key);
                }
            }
            foreach (KeyValuePair<ModuleItem, SlotItemView> pair2 in slotItems)
            {
                if (!registeredSlotItems.Contains(pair2.Key))
                {
                    SlotItemView view2;
                    registeredSlotItems.Add(pair2.Key);
                    pair2.Value.onClick = view2.onClick + new Action<SlotItemView>(this.OnItemClick);
                }
            }
        }
    }
}

