namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x16049ddf066L)]
    public class PersonalChatOwnerComponent : Component
    {
        public List<long> ChatsIs { get; set; }
    }
}

