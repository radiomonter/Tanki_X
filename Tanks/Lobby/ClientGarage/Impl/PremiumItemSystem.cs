namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientGarage.API;

    public class PremiumItemSystem : ECSSystem
    {
        [OnEventFire]
        public void OnAddPremiumBoostItem(NodeAddedEvent e, PremiumBoostItemNode item)
        {
        }

        [OnEventFire]
        public void OnAddPremiumQuestItem(NodeAddedEvent e, PremiumQuestItemNode item)
        {
        }

        [OnEventFire]
        public void OnAddPromoItem(NodeAddedEvent e, SingleNode<PremiumDurationChangedComponent> item)
        {
        }

        [OnEventFire]
        public void OnAddPromoItem(NodeAddedEvent e, SingleNode<PremiumPromoComponent> item)
        {
        }

        [OnEventFire]
        public void OnAddPromoItem(NodeRemoveEvent e, SingleNode<PremiumDurationChangedComponent> item)
        {
        }

        [OnEventFire]
        public void UserBoughtPremium(NodeAddedEvent e, SingleNode<PremiumWasBoughtComponent> user)
        {
        }

        public class PremiumBoostItemNode : Node
        {
            public PremiumBoostItemComponent premiumBoostItem;
            public DurationUserItemComponent durationUserItem;
        }

        public class PremiumQuestItemNode : Node
        {
            public PremiumQuestItemComponent premiumQuestItem;
            public DurationUserItemComponent durationUserItem;
        }
    }
}

