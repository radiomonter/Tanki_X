namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15741d0efb6L)]
    public class ExclusiveItemComponent : Component
    {
        public List<Publisher> ForbiddenForPublishers { get; set; }
    }
}

