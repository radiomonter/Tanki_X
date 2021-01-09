namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x1588b50bbcaL)]
    public class ItemsContainerItemComponent : Component
    {
        public List<ContainerItem> Items { get; set; }

        public List<ContainerItem> RareItems { get; set; }
    }
}

