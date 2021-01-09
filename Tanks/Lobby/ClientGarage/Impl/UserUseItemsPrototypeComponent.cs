namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x4ea6733b9b8cfe55L)]
    public class UserUseItemsPrototypeComponent : SharedChangeableComponent
    {
        public Entity PrototypeUser { get; set; }

        public Entity Preset { get; set; }
    }
}

