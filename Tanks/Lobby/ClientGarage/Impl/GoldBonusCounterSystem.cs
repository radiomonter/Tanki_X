namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;

    public class GoldBonusCounterSystem : ECSSystem
    {
        [OnEventFire]
        public void SetCounter(ItemsCountChangedEvent e, GoldBonusUserItemNode goldBonusItem, [JoinAll] SingleNode<GoldBonusCounterComponent> counter)
        {
            counter.component.SetCount(goldBonusItem.userItemCounter.Count);
        }

        [OnEventFire]
        public void SetCounter(NodeAddedEvent e, SingleNode<GoldBonusCounterComponent> counter, SelfUserNode user, [JoinByUser] GoldBonusUserItemNode goldBonusItem)
        {
            counter.component.SetCount(goldBonusItem.userItemCounter.Count);
        }

        [OnEventFire]
        public void SetCounter(GoldBonusesCountChangedEvent e, SelfUserNode user, [JoinByUser] GoldBonusUserItemNode goldBonusItem, [JoinAll] SingleNode<GoldBonusCounterComponent> counter)
        {
            counter.component.SetCount(e.NewCount);
        }

        public class GoldBonusUserItemNode : Node
        {
            public GoldBonusItemComponent goldBonusItem;
            public UserItemComponent userItem;
            public UserGroupComponent userGroup;
            public UserItemCounterComponent userItemCounter;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
        }
    }
}

