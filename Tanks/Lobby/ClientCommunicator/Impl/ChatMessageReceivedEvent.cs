namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x151d35e20c8L)]
    public class ChatMessageReceivedEvent : Event
    {
        public string Message { get; set; }

        public bool SystemMessage { get; set; }

        public string UserUid { get; set; }

        public long UserId { get; set; }

        public string UserAvatarId { get; set; }
    }
}

