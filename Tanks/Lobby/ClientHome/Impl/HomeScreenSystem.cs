namespace Tanks.Lobby.ClientHome.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientNavigation.Impl;
    using Tanks.Lobby.ClientSettings.API;

    public class HomeScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void FinalizeCompactWindow(NodeAddedEvent e, SingleNode<HomeScreenComponent> homeScreen)
        {
            GraphicsSettings.INSTANCE.DisableCompactScreen();
        }

        [OnEventFire]
        public void GroupWithUser(NodeAddedEvent e, SingleNode<MainScreenComponent> homeScreen, SelfUserNode selfUser)
        {
            SetScreenHeaderEvent eventInstance = new SetScreenHeaderEvent {
                Animate = false,
                Header = string.Empty
            };
            base.ScheduleEvent(eventInstance, homeScreen);
        }

        [OnEventFire]
        public void ShowHomeScreen(NodeRemoveEvent e, SingleNode<PreloadAllResourcesComponent> preloadAllResourcesRequest, [JoinAll] TopPanelNode topPanel)
        {
            base.ScheduleEvent<ShowScreenNoAnimationEvent<MainScreenComponent>>(topPanel);
        }

        [OnEventFire]
        public void ShowSettingsScreen(ButtonClickEvent e, SingleNode<SettingsButtonComponent> node)
        {
            base.ScheduleEvent<ShowScreenLeftEvent<SettingsScreenComponent>>(node);
            MainScreenComponent.Instance.SendShowScreenStat(LogScreen.Settings);
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserUidComponent userUid;
            public UserGroupComponent userGroup;
        }

        public class TopPanelNode : Node
        {
            public TopPanelComponent topPanel;
            public TopPanelAuthenticatedComponent topPanelAuthenticated;
        }
    }
}

