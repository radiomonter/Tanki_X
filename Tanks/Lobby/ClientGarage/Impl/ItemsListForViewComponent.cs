namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ItemsListForViewComponent : Component
    {
        public ItemsListForViewComponent()
        {
        }

        public ItemsListForViewComponent(List<Entity> items)
        {
            this.Items = items;
        }

        public List<Entity> Items { get; set; }
    }
}

