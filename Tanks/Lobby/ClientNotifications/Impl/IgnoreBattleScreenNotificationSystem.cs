namespace Tanks.Lobby.ClientNotifications.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientNotifications.API;
    using Tanks.Lobby.ClientUserProfile.Impl;

    public class IgnoreBattleScreenNotificationSystem : ECSSystem
    {
        [OnEventFire]
        public void CloseNotificationOnBattleScreen(NodeAddedEvent evt, [Combine] ActiveNotificationNode notification, BattleScreenNode screen)
        {
            base.ScheduleEvent<CloseNotificationEvent>(notification);
        }

        [OnEventFire]
        public void SkipNotificationOnBattleScreen(ShowNotificationEvent evt, SingleNode<IgnoreBattleScreenNotificationComponent> notification, [JoinAll] BattleScreenNode screen)
        {
            evt.CanShowNotification = false;
        }

        public class ActiveNotificationNode : Node
        {
            public ActiveNotificationComponent activeNotification;
            public IgnoreBattleScreenNotificationComponent ignoreBattleScreenNotification;
        }

        public class BattleScreenNode : Node
        {
            public ScreenComponent screen;
            public BattleScreenComponent battleScreen;
            public ActiveScreenComponent activeScreen;
        }
    }
}

