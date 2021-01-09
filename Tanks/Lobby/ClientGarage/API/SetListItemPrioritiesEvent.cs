namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SetListItemPrioritiesEvent : Event
    {
        public SetListItemPrioritiesEvent()
        {
            this.Priorities = new ListItemPriorities();
        }

        public ListItemPriorities Priorities { get; set; }
    }
}

