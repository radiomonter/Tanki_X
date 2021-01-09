namespace Tanks.Lobby.ClientFriends.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class FriendsScreenComponent : BehaviourComponent
    {
        private int acceptFriendsCounter;
        private int rejectFriendsCounter;
        private const int actionsForShowButton = 5;
        [SerializeField]
        private FriendsListUIComponent friendsList;

        public void AcceptFriend()
        {
            this.rejectFriendsCounter = 0;
            this.acceptFriendsCounter++;
            if (this.acceptFriendsCounter >= 5)
            {
                this.friendsList.EnableAddAllButton();
            }
        }

        public void AddItem(long userId, string userUid, FriendType friendType)
        {
            this.friendsList.AddItem(userId, userUid, friendType);
        }

        public void ClearIncoming(bool moveToAccepted)
        {
            this.friendsList.ClearIncoming(moveToAccepted);
        }

        public void Hide()
        {
            this.friendsList.Hide();
        }

        public void HideImmediate()
        {
            this.friendsList.HideImmediate();
        }

        public void RejectFriend()
        {
            this.acceptFriendsCounter = 0;
            this.rejectFriendsCounter++;
            if (this.rejectFriendsCounter >= 5)
            {
                this.friendsList.EnableRejectAllButton();
            }
        }

        public void RemoveUser(long userId, bool toRight)
        {
            this.friendsList.RemoveItem(userId, toRight);
        }

        public void ResetButtons()
        {
            this.friendsList.ResetButtons();
        }

        public void Show()
        {
            this.friendsList.Show(null);
            this.acceptFriendsCounter = 0;
            this.rejectFriendsCounter = 0;
        }
    }
}

