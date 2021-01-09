using System;
using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragAndDropCell : MonoBehaviour, IDropHandler, IEventSystemHandler
{
    public IDropController dropController;

    public DragAndDropItem GetItem() => 
        base.GetComponentInChildren<DragAndDropItem>();

    public void OnDrop(PointerEventData data)
    {
        if ((DragAndDropItem.draggedItemContentCopy != null) && DragAndDropItem.draggedItemContentCopy.activeSelf)
        {
            DragAndDropItem draggedItem = DragAndDropItem.draggedItem;
            DragAndDropCell sourceCell = DragAndDropItem.sourceCell;
            this.dropController.OnDrop(sourceCell, this, draggedItem);
        }
    }

    public void PlaceItem(DragAndDropItem item)
    {
        if (item != null)
        {
            item.transform.SetParent(base.transform, false);
            item.transform.localPosition = Vector3.zero;
        }
        base.gameObject.SendMessageUpwards("OnItemPlace", item, SendMessageOptions.DontRequireReceiver);
    }

    public void RemoveItem()
    {
        foreach (DragAndDropItem item in base.GetComponentsInChildren<DragAndDropItem>())
        {
            Destroy(item.gameObject);
        }
    }

    public void SwapItems(DragAndDropCell sourceCell, DragAndDropItem item)
    {
        DragAndDropItem item2 = this.GetItem();
        this.PlaceItem(item);
        if (item2 != null)
        {
            sourceCell.PlaceItem(item2);
        }
    }
}

