namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class RecievedLobbyChatMessageEvent : Event
    {
        public RecievedLobbyChatMessageEvent(ChatMessage message)
        {
            this.Message = message;
        }

        public ChatMessage Message { get; set; }
    }
}

