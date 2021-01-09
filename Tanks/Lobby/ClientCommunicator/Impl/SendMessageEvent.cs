namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SendMessageEvent : Event
    {
        public SendMessageEvent(string message)
        {
            this.Message = message;
        }

        public string Message { get; set; }
    }
}

