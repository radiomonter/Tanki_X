namespace Tanks.Lobby.ClientNotifications.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientNotifications.API;
    using Tanks.Lobby.ClientUserProfile.Impl;

    public class LockScreenNotificationSystem : ECSSystem
    {
        [OnEventFire]
        public void LockScreen(NodeRemoveEvent e, ActiveNotificationNode notification)
        {
            if ((notification.lockScreenNotification.ScreenEntity != null) && notification.lockScreenNotification.ScreenEntity.HasComponent<LockedScreenComponent>())
            {
                notification.lockScreenNotification.ScreenEntity.RemoveComponent<LockedScreenComponent>();
            }
        }

        [OnEventFire]
        public void LockScreen(NodeAddedEvent e, [Combine] ActiveNotificationNode notification, ScreenNode screen)
        {
            if (!screen.Entity.HasComponent<LockedScreenComponent>())
            {
                screen.Entity.AddComponent<LockedScreenComponent>();
                notification.lockScreenNotification.ScreenEntity = screen.Entity;
            }
        }

        public class ActiveNotificationNode : Node
        {
            public ActiveNotificationComponent activeNotification;
            public LockScreenNotificationComponent lockScreenNotification;
        }

        public class ScreenNode : Node
        {
            public ScreenComponent screen;
            public ActiveScreenComponent activeScreen;
        }
    }
}

