namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System.Collections.Generic;
    using UnityEngine;

    public class InviteFriendsListComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject friendsListItem;
        [SerializeField]
        private GameObject emptyListNotification;
        private List<string> friendsUids = new List<string>();

        public GameObject FriendsListItem =>
            this.friendsListItem;

        public GameObject EmptyListNotification =>
            this.emptyListNotification;

        public List<string> FriendsUids =>
            this.friendsUids;
    }
}

