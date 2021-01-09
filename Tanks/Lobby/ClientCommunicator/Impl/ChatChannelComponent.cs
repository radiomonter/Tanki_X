namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ChatChannelComponent : Component
    {
        private const int MAX_MESSAGES = 50;

        public ChatChannelComponent(Tanks.Lobby.ClientCommunicator.Impl.ChatType chatType)
        {
            this.ChatType = chatType;
            this.Messages = new List<ChatMessage>();
        }

        public void AddMessage(ChatMessage message)
        {
            if (this.Messages.Count > 50)
            {
                this.Messages.RemoveAt(0);
            }
            this.Messages.Add(message);
        }

        public Tanks.Lobby.ClientCommunicator.Impl.ChatType ChatType { get; private set; }

        public List<ChatMessage> Messages { get; private set; }
    }
}

