namespace Tanks.Lobby.ClientCommunicator.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x150ae7033a9L)]
    public class SendChatMessageEvent : Event
    {
        public SendChatMessageEvent()
        {
        }

        public SendChatMessageEvent(string message)
        {
            this.Message = message;
        }

        public string Message { get; set; }
    }
}

