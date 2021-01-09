namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientProfile.API;

    public class XPriceLabelSystem : AbstractPriceLabelSystem
    {
        [OnEventFire]
        public void PriceChanged(PriceChangedEvent e, PriceForUserNode priceForUser, [JoinAll, Mandatory] UserNode user)
        {
            base.UpdatePriceForUser(e.XPrice, e.OldXPrice, priceForUser.xPriceLabel, user.userXCrystals.Money);
        }

        [OnEventFire]
        public void SetPriceForUser(SetPriceEvent e, PriceForUserNode priceForUser, [JoinAll, Mandatory] UserNode user)
        {
            base.UpdatePriceForUser(e.XPrice, e.OldXPrice, priceForUser.xPriceLabel, user.userXCrystals.Money);
        }

        [OnEventFire]
        public void UpdateUserMoney(UserXCrystalsChangedEvent e, UserNode userCrystal, [JoinAll, Combine] SingleNode<PriceLabelComponent> price)
        {
            base.UpdatePriceForUser(price.component.Price, price.component.OldPrice, price.component, userCrystal.userXCrystals.Money);
        }

        public class PriceForUserNode : Node
        {
            public XPriceLabelComponent xPriceLabel;
        }

        public class UserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserXCrystalsComponent userXCrystals;
        }
    }
}

