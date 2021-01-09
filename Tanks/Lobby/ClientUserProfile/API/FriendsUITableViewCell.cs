namespace Tanks.Lobby.ClientUserProfile.API
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class FriendsUITableViewCell : UITableViewCell
    {
        [SerializeField]
        private UserListItemComponent friendsListItem;

        public void Init(long userId, bool delayedLoading)
        {
            this.friendsListItem.ResetItem(userId, delayedLoading);
        }

        public long id =>
            this.friendsListItem.userId;
    }
}

