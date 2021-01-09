namespace Tanks.Lobby.ClientNotifications.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientUserProfile.API;

    public class CancelNotificationSystem : ECSSystem
    {
        [OnEventFire]
        public void CloseActiveNotificationEvent(CloseNotificationEvent evt, SingleNode<CancelNotificationComponent> notification)
        {
            notification.component.enabled = false;
        }

        [OnEventFire]
        public void InitCancelBehaviour(NodeAddedEvent e, SingleNode<CancelNotificationComponent> notification)
        {
            notification.component.Init(notification.Entity);
        }
    }
}

