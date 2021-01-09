namespace Tanks.Lobby.ClientCommunicator.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ChatConfigComponent : Component
    {
        public int MaxMessageLength { get; set; }
    }
}

