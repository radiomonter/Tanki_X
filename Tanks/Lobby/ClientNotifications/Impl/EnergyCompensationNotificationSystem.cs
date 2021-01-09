namespace Tanks.Lobby.ClientNotifications.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientNotifications.API;

    public class EnergyCompensationNotificationSystem : ECSSystem
    {
        [OnEventFire]
        public void Fill(NodeAddedEvent e, EnergyCompensationNotificationNode notification, SingleNode<EnergyCompensationDialog> dialogNode, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            CheckForTutorialEvent eventInstance = new CheckForTutorialEvent();
            base.ScheduleEvent(eventInstance, notification);
            if (eventInstance.TutorialIsActive)
            {
                base.ScheduleEvent<NotificationShownEvent>(notification);
            }
            else
            {
                EnergyCompensationDialog component = dialogNode.component;
                List<Animator> animators = !screens.IsPresent() ? new List<Animator>() : screens.Get().component.Animators;
                component.Show(notification.energyCompensationNotification.Charges, notification.energyCompensationNotification.Crys, animators);
            }
        }

        private void HidePopup([JoinAll] EnergyCompensationNotificationNode notification, [JoinAll] Optional<SingleNode<EnergyCompensationDialog>> popup)
        {
            if (popup.IsPresent())
            {
                popup.Get().component.Hide();
            }
            base.ScheduleEvent<NotificationShownEvent>(notification);
        }

        [OnEventFire]
        public void HidePopup(ButtonClickEvent e, SingleNode<EnergyCompensationDialogCloseButton> button, [JoinAll] EnergyCompensationNotificationNode notification, [JoinAll] Optional<SingleNode<EnergyCompensationDialog>> popup)
        {
            this.HidePopup(notification, popup);
        }

        [OnEventFire]
        public void HidePopup(ShowTutorialStepEvent e, Node any, [JoinAll] EnergyCompensationNotificationNode notification, [JoinAll] Optional<SingleNode<EnergyCompensationDialog>> popup)
        {
            this.HidePopup(notification, popup);
        }

        public class EnergyCompensationNotificationNode : Node
        {
            public EnergyCompensationNotificationComponent energyCompensationNotification;
            public ResourceDataComponent resourceData;
        }
    }
}

