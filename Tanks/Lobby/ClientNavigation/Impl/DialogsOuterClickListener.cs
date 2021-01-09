namespace Tanks.Lobby.ClientNavigation.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine.EventSystems;

    public class DialogsOuterClickListener : UIBehaviour, IPointerClickHandler, IEventSystemHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            this.ClickAction(eventData);
        }

        public Action<PointerEventData> ClickAction { get; set; }
    }
}

