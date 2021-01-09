namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class GraffitiSorter : MonoBehaviour, SimpleHorizontalListItemsSorter, IComparer<ListItem>
    {
        public int Compare(ListItem x, ListItem y)
        {
            Entity data = x.Data as Entity;
            Entity entity = y.Data as Entity;
            if ((data == null) || (entity == null))
            {
                return 0;
            }
            bool flag = this.IsUserItem(data);
            bool flag2 = this.IsUserItem(entity);
            if (!flag || !flag2)
            {
                return ((flag || flag2) ? (!flag ? (!this.IsLocked(entity) ? -1 : 1) : (!this.IsLocked(data) ? -1 : 1)) : ((this.GetId(data) >= this.GetId(entity)) ? 1 : -1));
            }
            bool flag4 = this.IsLocked(entity);
            return (!(this.IsLocked(data) ^ flag4) ? ((this.GetId(data) >= this.GetId(entity)) ? 1 : -1) : (!flag4 ? 1 : -1));
        }

        private int GetId(Entity entity) => 
            entity.GetComponent<OrderItemComponent>().Index;

        private bool IsLocked(Entity entity) => 
            entity.HasComponent<RestrictedByUpgradeLevelComponent>();

        private bool IsUserItem(Entity entity) => 
            entity.HasComponent<UserItemComponent>();

        public void Sort(ItemsMap items)
        {
            items.Sort(this);
        }
    }
}

