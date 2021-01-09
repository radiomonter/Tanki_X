namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientUserProfile.API;

    public class EmailConfirmationNotificationSystem : ECSSystem
    {
        private bool emailIsEmpty(Entity user) => 
            !user.HasComponent<UnconfirmedUserEmailComponent>() || string.IsNullOrEmpty(user.GetComponent<UnconfirmedUserEmailComponent>().Email);

        [OnEventFire]
        public void PrepareNotificationText(NodeAddedEvent e, SingleNode<EmailConfirmationNotificationComponent> notification, [JoinAll] HomeScreenNode activeScreen, [JoinAll] SingleNode<SelfUserComponent> selfUser)
        {
            if (this.emailIsEmpty(selfUser.Entity))
            {
                notification.Entity.AddComponent(new NotificationMessageComponent(notification.component.ChangeEmailMessageTemplate));
            }
            else
            {
                notification.Entity.AddComponent(new NotificationMessageComponent(notification.component.ConfirmationMessageTemplate));
            }
        }

        public class HomeScreenNode : Node
        {
            public ActiveScreenComponent activeScreen;
            public HomeScreenComponent homeScreen;
        }
    }
}

