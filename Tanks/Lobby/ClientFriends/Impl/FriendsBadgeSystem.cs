namespace Tanks.Lobby.ClientFriends.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientFriends.API;
    using UnityEngine;

    public class FriendsBadgeSystem : ECSSystem
    {
        [OnEventFire]
        public void AddAcceptedFriend(AcceptedFriendAddedEvent e, Node any, [JoinAll] SingleNode<IncomingFriendsCounterComponent> counter)
        {
            counter.component.Count--;
        }

        [OnEventFire]
        public void AddAcceptedFriend(AcceptedFriendAddedEvent e, SingleNode<FriendsBadgeCounterComponent> friends, [JoinAll] Optional<SingleNode<OpenFriendsScreenButtonComponent>> button)
        {
            friends.component.Counter--;
            this.UpdateBadgeIfPresent(friends, button);
        }

        [OnEventFire]
        public void AddBadgeOnEnter(NodeAddedEvent e, SingleNode<OpenFriendsScreenButtonComponent> button, SingleNode<FriendsBadgeCounterComponent> friends)
        {
            this.Update(button, friends);
        }

        [OnEventFire]
        public void AddCounter(NodeAddedEvent e, SingleNode<FriendsComponent> friends)
        {
            FriendsBadgeCounterComponent component = new FriendsBadgeCounterComponent {
                Counter = friends.component.IncommingFriendsIds.Count
            };
            friends.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void AddIncommingFriend(IncomingFriendAddedEvent e, SingleNode<FriendsBadgeCounterComponent> friends, [JoinAll] Optional<SingleNode<OpenFriendsScreenButtonComponent>> button)
        {
            friends.component.Counter++;
            this.UpdateBadgeIfPresent(friends, button);
        }

        [OnEventFire]
        public void HideBadgeOnEnterScreen(ButtonClickEvent e, SingleNode<OpenFriendsScreenButtonComponent> button, [JoinAll] SingleNode<FriendsBadgeCounterComponent> friends)
        {
            friends.component.Counter = 0;
            this.Update(button, friends);
        }

        [OnEventFire]
        public void RejectAll(RejectAllFriendsEvent e, Node any, [JoinAll] SingleNode<IncomingFriendsCounterComponent> counter)
        {
            counter.component.Count = 0;
        }

        [OnEventFire]
        public void RejectFriend(RejectFriendEvent e, Node any, [JoinAll] SingleNode<IncomingFriendsCounterComponent> counter)
        {
            counter.component.Count--;
        }

        [OnEventFire]
        public void SetCountOnEnter(NodeAddedEvent e, SingleNode<IncomingFriendsCounterComponent> counter, [JoinAll] SingleNode<FriendsComponent> friends)
        {
            counter.component.Count = friends.component.IncommingFriendsIds.Count;
        }

        private void Update(SingleNode<OpenFriendsScreenButtonComponent> button, SingleNode<FriendsBadgeCounterComponent> friends)
        {
            if (friends.component.Counter <= 0)
            {
                base.Log.Info("HideBadge");
                friends.component.Counter = Mathf.Max(0, friends.component.Counter);
                button.component.countingBadge.SetActive(false);
            }
            else
            {
                base.Log.Info("ShowBadge");
                button.component.countingBadge.Count = friends.component.Counter;
                button.component.countingBadge.SetActive(true);
            }
        }

        private void UpdateBadgeIfPresent(SingleNode<FriendsBadgeCounterComponent> friends, Optional<SingleNode<OpenFriendsScreenButtonComponent>> button)
        {
            if (button.IsPresent())
            {
                this.Update(button.Get(), friends);
            }
        }
    }
}

