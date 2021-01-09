namespace Tanks.Lobby.ClientGarage.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientEntrance.API;

    public class QuickRegistrartionNavigationSystem : ECSSystem
    {
        [OnEventFire]
        public void Complete(NodeRemoveEvent e, SingleNode<UserIncompleteRegistrationComponent> user, [JoinAll] SingleNode<RegistrationScreenComponent> screen)
        {
            base.NewEvent<ShowScreenDownEvent<HomeScreenComponent>>().Attach(screen).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void Complete(ButtonClickEvent e, SingleNode<BackButtonComponent> user, [JoinAll] SingleNode<RegistrationScreenComponent> screen, [JoinAll] UserWithIncompleteRegNode userWithIncompleteReg)
        {
            MainScreenComponent.Instance.ShowMain();
            base.NewEvent<ShowScreenDownEvent<HomeScreenComponent>>().Attach(screen).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void DisableShop(NodeAddedEvent e, SingleNode<ShopComponent> homeScreen, [JoinAll] UserWithIncompleteRegNode userWithIncompleteReg)
        {
            MainScreenComponent.Instance.ShowMain();
        }

        public bool IsRegistartionTime(UserWithIncompleteRegNode userWithIncompleteReg) => 
            userWithIncompleteReg.userIncompleteRegistration.FirstBattleDone;

        [OnEventComplete]
        public void ShowOnMainScreen(NodeAddedEvent e, SingleNode<MainScreenComponent> homeScreen, UserWithIncompleteRegNode userWithIncompleteReg)
        {
            if (this.IsRegistartionTime(userWithIncompleteReg))
            {
                base.NewEvent<DelayedShowRegistrationEvent>().Attach(homeScreen).ScheduleDelayed(0f);
            }
        }

        [OnEventComplete]
        public void ShowOnMainScreenAndNotificationLeave(NodeRemoveEvent e, SingleNode<ActiveNotificationComponent> activeNotification, [JoinAll] SingleNode<MainScreenComponent> homeScreen, UserWithIncompleteRegNode userWithIncompleteReg, [JoinAll] ICollection<SingleNode<ActiveNotificationComponent>> activeNotifications)
        {
            if (this.IsRegistartionTime(userWithIncompleteReg) && (activeNotifications.Count == 1))
            {
                base.NewEvent<ShowScreenDownEvent<RegistrationScreenComponent>>().Attach(homeScreen).ScheduleDelayed(0f);
            }
        }

        [OnEventFire]
        public void ShowOnMainScreenDelayed(DelayedShowRegistrationEvent e, SingleNode<MainScreenComponent> homeScreen, [JoinAll] UserWithIncompleteRegNode userWithIncompleteReg, [JoinAll] ICollection<SingleNode<ActiveNotificationComponent>> activeNotifications)
        {
            if (this.IsRegistartionTime(userWithIncompleteReg) && (activeNotifications.Count == 0))
            {
                base.NewEvent<ShowScreenDownEvent<RegistrationScreenComponent>>().Attach(homeScreen).ScheduleDelayed(0f);
            }
        }

        [OnEventFire]
        public void ShowOnShopScreenDelayed(NodeAddedEvent e, SingleNode<ShopComponent> homeScreen, [JoinAll] UserWithIncompleteRegNode userWithIncompleteReg)
        {
            base.NewEvent<ShowScreenDownEvent<RegistrationScreenComponent>>().Attach(homeScreen).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void ShowOnShopUserProfileDelayed(NodeAddedEvent e, SingleNode<ProfileScreenComponent> homeScreen, [JoinAll] UserWithIncompleteRegNode userWithIncompleteReg)
        {
            base.NewEvent<ShowScreenDownEvent<RegistrationScreenComponent>>().Attach(homeScreen).ScheduleDelayed(0f);
        }

        public class DelayedShowRegistrationEvent : Event
        {
        }

        public class UserWithIncompleteRegNode : Node
        {
            public UserIncompleteRegistrationComponent userIncompleteRegistration;
            public UserRankComponent userRank;
            public SelfUserComponent selfUser;
        }
    }
}

