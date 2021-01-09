namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine.EventSystems;

    public class ChatMessageClickEvent : Event
    {
        public PointerEventData EventData { get; set; }

        public string Link { get; set; }
    }
}

