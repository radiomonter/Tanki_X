namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientUserProfile.API;

    public class ServerShutdownNotificationSystem : ECSSystem
    {
        [OnEventFire]
        public void PrepareNotificationText(NodeAddedEvent e, NotificationNode notification)
        {
            notification.Entity.AddComponent(new NotificationMessageComponent(string.Empty));
        }

        [OnEventFire]
        public void UpdateTimeNotification(UpdateEvent e, NotificationMessageNode notification, [JoinAll] ShutdownNode shutdown, [Combine, JoinAll] SingleNode<LocalizedTimerComponent> timer)
        {
            float time = shutdown.serverShutdown.StopDateForClient.UnityTime - Date.Now.UnityTime;
            if (time < 0f)
            {
                time = 0f;
            }
            string str = timer.component.GenerateTimerString(time);
            notification.notificationMessage.Message = string.Format(notification.serverShutdownText.Text, str);
        }

        public class NotificationMessageNode : Node
        {
            public ServerShutdownNotificationComponent serverShutdownNotification;
            public ServerShutdownTextComponent serverShutdownText;
            public NotificationMessageComponent notificationMessage;
        }

        public class NotificationNode : Node
        {
            public ServerShutdownNotificationComponent serverShutdownNotification;
            public ServerShutdownTextComponent serverShutdownText;
        }

        public class ShutdownNode : Node
        {
            public ServerShutdownComponent serverShutdown;
        }
    }
}

