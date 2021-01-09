namespace Tanks.Lobby.ClientNavigation.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine.EventSystems;

    public class DialogsOuterClickEvent : Event
    {
        public PointerEventData EventData { get; set; }
    }
}

