namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class MountItemSystem : ECSSystem
    {
        [OnEventFire]
        public void Crutch(MountItemEvent e, Node any)
        {
        }

        [OnEventFire]
        public void MountItem(ButtonClickEvent e, SingleNode<MountItemButtonComponent> button, [JoinByScreen] SingleNode<SelectedItemComponent> selectedItem)
        {
            base.ScheduleEvent<MountItemEvent>(selectedItem.component.SelectedItem);
            base.ScheduleEvent<MountParentItemEvent>(selectedItem.component.SelectedItem);
        }

        [OnEventFire]
        public void MountParentItem(MountParentItemEvent e, SkinUserItemNode skinUserItem, [JoinByParentGroup, Combine] NotMountedUserItemNode parentUserItem, [JoinByMarketItem] MarketItemNode parentMarketItemNode)
        {
            if (parentMarketItemNode.Entity.Id == skinUserItem.parentGroup.Key)
            {
                base.ScheduleEvent<MountItemEvent>(parentUserItem);
            }
        }

        public class MarketItemNode : Node
        {
            public MarketItemComponent marketItem;
            public MarketItemGroupComponent marketItemGroup;
        }

        public class MountParentItemEvent : Event
        {
        }

        [Not(typeof(MountedItemComponent))]
        public class NotMountedUserItemNode : MountItemSystem.UserItemNode
        {
        }

        public class SkinUserItemNode : MountItemSystem.UserItemNode
        {
            public SkinItemComponent skinItem;
        }

        public class UserItemNode : Node
        {
            public UserItemComponent userItem;
            public ParentGroupComponent parentGroup;
            public MarketItemGroupComponent marketItemGroup;
        }
    }
}

