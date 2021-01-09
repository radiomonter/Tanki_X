namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientProfile.API;

    public class BuyEnergySystem : ECSSystem
    {
        [CompilerGenerated]
        private static Action <>f__am$cache0;

        [OnEventFire]
        public void OnPressEnergyContextBuyButton(PressEnergyContextBuyButtonEvent e, Node any, [JoinAll] UserNode selfUser, [JoinAll] SingleNode<Dialogs60Component> dialogs)
        {
            if (selfUser.userXCrystals.Money >= e.XPrice)
            {
                base.NewEvent(new EnergyContextBuyEvent(e.Count, e.XPrice)).Attach(selfUser).Schedule();
            }
            else
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = delegate {
                    };
                }
                dialogs.component.Get<BuyConfirmationDialog>().XShow(null, <>f__am$cache0, (int) e.XPrice, 1, null, false, null);
            }
        }

        public class UserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserXCrystalsComponent userXCrystals;
        }
    }
}

