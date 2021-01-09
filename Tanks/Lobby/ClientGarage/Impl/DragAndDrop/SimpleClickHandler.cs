namespace Tanks.Lobby.ClientGarage.Impl.DragAndDrop
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class SimpleClickHandler : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
    {
        public Action<GameObject> onClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (this.onClick != null)
            {
                this.onClick(base.gameObject);
            }
        }
    }
}

