namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    public class UpgradeRewardsSorter : MonoBehaviour, SimpleHorizontalListItemsSorter, IComparer<ListItem>
    {
        public int Compare(ListItem x, ListItem y)
        {
            Entity data = x.Data as Entity;
            Entity entity = y.Data as Entity;
            if ((data == null) || (entity == null))
            {
                return 0;
            }
            AbstractRestrictionComponent restriction = this.GetRestriction(data);
            AbstractRestrictionComponent component2 = this.GetRestriction(entity);
            return (((restriction != null) || (component2 != null)) ? ((restriction != null) ? ((component2 != null) ? restriction.RestrictionValue.CompareTo(component2.RestrictionValue) : 1) : -1) : 0);
        }

        private AbstractRestrictionComponent GetRestriction(Entity entity)
        {
            AbstractRestrictionComponent restriction = null;
            if (entity.HasComponent<PurchaseUpgradeLevelRestrictionComponent>())
            {
                restriction = entity.GetComponent<PurchaseUpgradeLevelRestrictionComponent>();
                if (restriction.RestrictionValue > 0)
                {
                    return restriction;
                }
                restriction = null;
            }
            if (entity.HasComponent<MountUpgradeLevelRestrictionComponent>())
            {
                restriction = entity.GetComponent<MountUpgradeLevelRestrictionComponent>();
                if (restriction.RestrictionValue > 0)
                {
                    return restriction;
                }
                restriction = null;
            }
            if (entity.HasComponent<UserItemComponent>() && entity.HasComponent<MarketItemGroupComponent>())
            {
                Entity entity2 = Flow.Current.EntityRegistry.GetEntity(entity.GetComponent<MarketItemGroupComponent>().Key);
                restriction = this.GetRestriction(entity2);
            }
            if (entity.HasComponent<SlotUserItemInfoComponent>())
            {
                MountUpgradeLevelRestrictionComponent component2 = new MountUpgradeLevelRestrictionComponent {
                    RestrictionValue = entity.GetComponent<SlotUserItemInfoComponent>().UpgradeLevel
                };
                restriction = component2;
                if (restriction.RestrictionValue > 0)
                {
                    return restriction;
                }
                restriction = null;
            }
            return restriction;
        }

        public void Sort(ItemsMap items)
        {
            items.Sort(this);
        }
    }
}

