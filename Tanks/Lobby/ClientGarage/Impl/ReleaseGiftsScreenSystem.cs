namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNotifications.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class ReleaseGiftsScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void Fill(NodeAddedEvent e, ReleaseGiftsNotificationNode notification, SingleNode<ReleaseGiftsPopup> popup, [JoinAll] UserNode user, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens)
        {
            ReleaseGiftsPopup popup2 = popup.component;
            popup2.itemsContainer.DestroyChildren();
            int num = 0;
            foreach (KeyValuePair<long, int> pair in notification.releaseGiftsNotification.Reward)
            {
                popup2.itemPrefab.GetComponent<AnimationTriggerDelayBehaviour>().dealy = (num + 1) * popup2.itemsShowDelay;
                ReleaseGiftItemComponent component = Object.Instantiate<ReleaseGiftItemComponent>(popup2.itemPrefab, popup2.itemsContainer, false);
                Entity entity = Flow.Current.EntityRegistry.GetEntity(pair.Key);
                int num2 = pair.Value;
                component.preview.SpriteUid = entity.GetComponent<ImageItemComponent>().SpriteUid;
                bool flag = num2 > 1;
                component.text.text = entity.GetComponent<DescriptionItemComponent>().Name + (!flag ? string.Empty : " x");
                component.gameObject.SetActive(true);
                if (flag)
                {
                    component.count.Value = num2;
                }
                else
                {
                    component.count.gameObject.SetActive(false);
                }
                num++;
            }
            popup2.Show(!screens.IsPresent() ? new List<Animator>() : screens.Get().component.Animators);
        }

        [OnEventFire]
        public void HidePopup(ButtonClickEvent e, SingleNode<ReleaseGiftsPopupCloseButtonComponent> button, [JoinAll] ReleaseGiftsNotificationNode notification, [JoinAll] SingleNode<ReleaseGiftsPopup> popup)
        {
            popup.component.Hide();
            base.ScheduleEvent<NotificationShownEvent>(notification);
        }

        [OnEventFire]
        public void SetRewardInfo(NodeAddedEvent e, ReleaseGiftsNotificationNode notification)
        {
            notification.Entity.AddComponent(new NotificationMessageComponent(string.Empty));
        }

        public class ReleaseGiftsNotificationNode : Node
        {
            public ReleaseGiftsNotificationComponent releaseGiftsNotification;
            public ResourceDataComponent resourceData;
        }

        public class UserNode : Node
        {
            public SelfUserComponent selfUser;
        }
    }
}

