namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class GetAllItemsEvent<T> : Event where T: GarageItem
    {
        public GetAllItemsEvent()
        {
            this.Items = new List<T>();
        }

        public List<T> Items { get; set; }
    }
}

