namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientProfile.API;

    public class GaragePriceSystem : ECSSystem
    {
        [OnEventFire]
        public void UpdatePrices(UserMoneyChangedEvent e, SelfUserNode selfUser)
        {
            GaragePrice.UpdatePrices();
        }

        [OnEventFire]
        public void UpdatePrices(UserXCrystalsChangedEvent e, SelfUserNode selfUser)
        {
            GaragePrice.UpdatePrices();
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
        }
    }
}

