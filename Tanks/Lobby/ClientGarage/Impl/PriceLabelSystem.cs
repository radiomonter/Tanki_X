namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;

    public class PriceLabelSystem : AbstractPriceLabelSystem
    {
        [OnEventFire]
        public void PriceChanged(PriceChangedEvent e, PriceForUserNode priceForUser, [JoinAll, Mandatory] UserNode user)
        {
            base.UpdatePriceForUser(e.Price, e.OldPrice, priceForUser.priceLabel, user.userMoney.Money);
        }

        [OnEventFire]
        public void SetPriceForUser(SetPriceEvent e, PriceForUserNode priceForUser, [JoinAll, Mandatory] UserNode user)
        {
            base.UpdatePriceForUser(e.Price, e.OldPrice, priceForUser.priceLabel, user.userMoney.Money);
        }

        [OnEventFire]
        public void UpdateUserMoney(UserMoneyChangedEvent e, UserNode userCrystal, [JoinAll, Combine] SingleNode<PriceLabelComponent> price)
        {
            base.UpdatePriceForUser(price.component.Price, price.component.OldPrice, price.component, userCrystal.userMoney.Money);
        }

        public class PriceForUserNode : Node
        {
            public PriceLabelComponent priceLabel;
        }

        public class UserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserMoneyComponent userMoney;
        }
    }
}

