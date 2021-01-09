namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1540a161e8aL)]
    public class RestorePasswordUserDataComponent : Component
    {
        public string Uid { get; set; }
    }
}

