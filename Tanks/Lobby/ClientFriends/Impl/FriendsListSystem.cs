namespace Tanks.Lobby.ClientFriends.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;

    public class FriendsListSystem : ECSSystem
    {
        [OnEventFire]
        public void MarkListItemDeselected(ListItemDeselectedEvent e, SelectedListItemNode item)
        {
            item.Entity.RemoveComponent<SelectedListItemComponent>();
        }

        [OnEventFire]
        public void MarkListItemSelected(ListItemSelectedEvent e, NotSelectedListItemNode item)
        {
            item.Entity.AddComponent<SelectedListItemComponent>();
        }

        [Not(typeof(SelectedListItemComponent))]
        public class NotSelectedListItemNode : Node
        {
            public FriendsListItemComponent friendsListItem;
        }

        public class SelectedListItemNode : Node
        {
            public SelectedListItemComponent selectedListItem;
            public FriendsListItemComponent friendsListItem;
        }
    }
}

