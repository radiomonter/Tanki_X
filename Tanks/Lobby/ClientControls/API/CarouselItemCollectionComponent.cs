namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CarouselItemCollectionComponent : Component
    {
        public List<Entity> Items { get; set; }
    }
}

