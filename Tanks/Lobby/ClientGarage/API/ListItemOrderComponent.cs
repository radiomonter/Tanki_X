namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ListItemOrderComponent : Component
    {
        public ListItemOrderComponent()
        {
            this.Priorities = new ListItemPriorities();
        }

        public ListItemPriorities Priorities { get; set; }
    }
}

