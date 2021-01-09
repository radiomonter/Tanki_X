namespace Tanks.Lobby.ClientNotifications.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientNotifications.API;

    public class EulaNotificationSystem : ECSSystem
    {
        [OnEventFire]
        public void Fill(NodeAddedEvent e, [Combine] EulaNotificationNode notification, SingleNode<EulaDialog> dialogNode, SingleNode<MainScreenComponent> mainScreen, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            dialogNode.component.Show(!screens.IsPresent() ? null : screens.Get().component.Animators);
        }

        [OnEventFire]
        public void HidePopup(ButtonClickEvent e, SingleNode<EulaAgreeButton> button, [JoinAll] EulaNotificationNode notification, [JoinAll] Optional<SingleNode<EulaDialog>> popup)
        {
            if (popup.IsPresent())
            {
                popup.Get().component.HideByAcceptButton();
            }
            base.ScheduleEvent<NotificationShownEvent>(notification);
        }

        public class EulaNotificationNode : Node
        {
            public EulaNotificationComponent eulaNotification;
            public ResourceDataComponent resourceData;
        }
    }
}

