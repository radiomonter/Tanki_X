namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15944edd3d4L)]
    public class UserSubscribeComponent : Component
    {
        public bool Subscribed { get; set; }
    }
}

