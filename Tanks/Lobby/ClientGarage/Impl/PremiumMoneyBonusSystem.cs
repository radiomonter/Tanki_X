namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientGarage.API;

    public class PremiumMoneyBonusSystem : ECSSystem
    {
        [OnEventFire]
        public void RegisterBonus(NodeAddedEvent e, MoneyBonusNode bonus)
        {
        }

        public class MoneyBonusNode : Node
        {
            public UserGroupComponent userGroup;
            public MoneyBonusComponent moneyBonus;
        }
    }
}

