namespace Tanks.Lobby.ClientNotifications.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientNotifications.API;

    public class PrivacyPolicyNotificationSystem : ECSSystem
    {
        [OnEventFire]
        public void Fill(NodeAddedEvent e, PrivacyPolicyNotificationNode notification, SingleNode<PrivacyPolicyDialog> dialogNode, SingleNode<MainScreenComponent> mainScreen, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            dialogNode.component.Show(!screens.IsPresent() ? new List<Animator>() : screens.Get().component.Animators);
        }

        [OnEventFire]
        public void HidePopup(ButtonClickEvent e, SingleNode<PrivacyPolicyOkButton> button, [JoinAll] PrivacyPolicyNotificationNode notification, [JoinAll] Optional<SingleNode<PrivacyPolicyDialog>> popup)
        {
            if (popup.IsPresent())
            {
                popup.Get().component.HideByAcceptButton();
            }
            base.ScheduleEvent<NotificationShownEvent>(notification);
        }

        public class PrivacyPolicyNotificationNode : Node
        {
            public PrivacyPolicyNotificationComponent privacyPolicyNotification;
            public ResourceDataComponent resourceData;
        }
    }
}

