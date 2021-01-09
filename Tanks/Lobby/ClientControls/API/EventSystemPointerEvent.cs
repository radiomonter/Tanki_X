namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine.EventSystems;

    public abstract class EventSystemPointerEvent : Event
    {
        protected EventSystemPointerEvent()
        {
        }

        public UnityEngine.EventSystems.PointerEventData PointerEventData { get; set; }
    }
}

