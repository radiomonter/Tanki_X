using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragAndDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
{
    public static DragAndDropItem draggedItem;
    public static GameObject draggedItemContentCopy;
    public static DragAndDropCell sourceCell;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static DragEvent OnItemDragStartEvent;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static DragEvent OnItemDragEndEvent;
    private GameObject itemContent;

    public static event DragEvent OnItemDragEndEvent
    {
        add
        {
            DragEvent onItemDragEndEvent = OnItemDragEndEvent;
            while (true)
            {
                DragEvent objB = onItemDragEndEvent;
                onItemDragEndEvent = Interlocked.CompareExchange<DragEvent>(ref OnItemDragEndEvent, objB + value, onItemDragEndEvent);
                if (ReferenceEquals(onItemDragEndEvent, objB))
                {
                    return;
                }
            }
        }
        remove
        {
            DragEvent onItemDragEndEvent = OnItemDragEndEvent;
            while (true)
            {
                DragEvent objB = onItemDragEndEvent;
                onItemDragEndEvent = Interlocked.CompareExchange<DragEvent>(ref OnItemDragEndEvent, objB - value, onItemDragEndEvent);
                if (ReferenceEquals(onItemDragEndEvent, objB))
                {
                    return;
                }
            }
        }
    }

    public static event DragEvent OnItemDragStartEvent
    {
        add
        {
            DragEvent onItemDragStartEvent = OnItemDragStartEvent;
            while (true)
            {
                DragEvent objB = onItemDragStartEvent;
                onItemDragStartEvent = Interlocked.CompareExchange<DragEvent>(ref OnItemDragStartEvent, objB + value, onItemDragStartEvent);
                if (ReferenceEquals(onItemDragStartEvent, objB))
                {
                    return;
                }
            }
        }
        remove
        {
            DragEvent onItemDragStartEvent = OnItemDragStartEvent;
            while (true)
            {
                DragEvent objB = onItemDragStartEvent;
                onItemDragStartEvent = Interlocked.CompareExchange<DragEvent>(ref OnItemDragStartEvent, objB - value, onItemDragStartEvent);
                if (ReferenceEquals(onItemDragStartEvent, objB))
                {
                    return;
                }
            }
        }
    }

    private void Awake()
    {
        this.itemContent = base.transform.GetChild(0).gameObject;
    }

    private GameObject GetCopy(GameObject draggedItemContent)
    {
        GameObject obj2 = Instantiate<GameObject>(draggedItemContent);
        obj2.layer = base.gameObject.layer;
        RectTransform component = obj2.GetComponent<RectTransform>();
        component.anchorMax = new Vector2(0.5f, 0.5f);
        component.anchorMin = new Vector2(0.5f, 0.5f);
        component.pivot = new Vector2(0.5f, 0.5f);
        component.anchoredPosition = Vector2.zero;
        Rect rect = draggedItemContent.GetComponent<RectTransform>().rect;
        component.sizeDelta = rect.size;
        obj2.SetActive(true);
        return obj2;
    }

    public void MakeVisible(bool condition)
    {
        this.itemContent.GetComponent<CanvasGroup>().alpha = !condition ? ((float) 0) : ((float) 1);
        this.itemContent.transform.GetChild(1).gameObject.SetActive(condition);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
        {
            sourceCell = base.GetComponentInParent<DragAndDropCell>();
            draggedItem = this;
            draggedItemContentCopy = this.GetCopy(this.itemContent);
            Canvas componentInParent = sourceCell.transform.parent.GetComponentInParent<Canvas>();
            if (componentInParent != null)
            {
                draggedItemContentCopy.transform.SetParent(componentInParent.transform, false);
                draggedItemContentCopy.transform.SetAsLastSibling();
            }
            this.MakeVisible(false);
            if (OnItemDragStartEvent != null)
            {
                OnItemDragStartEvent(this, eventData);
            }
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if ((data.button != PointerEventData.InputButton.Right) && (draggedItemContentCopy != null))
        {
            Vector2 vector;
            Canvas componentInParent = sourceCell.transform.parent.GetComponentInParent<Canvas>();
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(componentInParent.GetComponent<RectTransform>(), Input.mousePosition, componentInParent.worldCamera, out vector))
            {
                draggedItemContentCopy.GetComponent<RectTransform>().anchoredPosition = vector;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if ((eventData == null) || (eventData.button != PointerEventData.InputButton.Right))
        {
            if (draggedItemContentCopy != null)
            {
                Destroy(draggedItemContentCopy);
            }
            this.MakeVisible(true);
            if (OnItemDragEndEvent != null)
            {
                OnItemDragEndEvent(this, eventData);
            }
            draggedItem = null;
            draggedItemContentCopy = null;
            sourceCell = null;
        }
    }

    public delegate void DragEvent(DragAndDropItem item, PointerEventData eventData);
}

