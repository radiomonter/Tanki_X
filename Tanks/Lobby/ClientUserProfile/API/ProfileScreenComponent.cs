namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientNavigation.API;
    using TMPro;
    using UnityEngine;

    public class ProfileScreenComponent : BehaviourComponent, NoScaleScreen
    {
        [SerializeField]
        private TextMeshProUGUI otherPlayerNickname;
        [SerializeField]
        private GameObject addToFriendRow;
        [SerializeField]
        private GameObject friendRequestRow;
        [SerializeField]
        private GameObject revokeFriendRow;
        [SerializeField]
        private GameObject removeFriendRow;
        [SerializeField]
        private GameObject enterBattleAsSpectatorRow;
        [SerializeField]
        private ImageListSkin leagueBorder;
        [SerializeField]
        private ImageSkin avatar;
        [SerializeField]
        private GameObject _premiumFrame;
        public GameObject selfUserAccountButtonsTab;
        public GameObject otherUserAccountButtonsTab;
        [SerializeField]
        private Color friendColor;

        public void HideOnNewItemNotificationShow()
        {
            base.GetComponent<Animator>().SetBool("newItemNotification", true);
        }

        private void OnEnable()
        {
            this.AddToFriendRow.SetActive(false);
            this.friendRequestRow.SetActive(false);
            this.revokeFriendRow.SetActive(false);
            this.removeFriendRow.SetActive(false);
            this.enterBattleAsSpectatorRow.SetActive(false);
            this.otherPlayerNickname.gameObject.SetActive(false);
        }

        public void SetPlayerColor(bool playerIsFriend)
        {
            this.otherPlayerNickname.color = !playerIsFriend ? Color.white : this.friendColor;
        }

        public void ShowOnNewItemNotificationCLose()
        {
            base.GetComponent<Animator>().SetBool("newItemNotification", false);
        }

        public ImageListSkin LeagueBorder =>
            this.leagueBorder;

        public ImageSkin Avatar =>
            this.avatar;

        public bool IsPremium
        {
            set => 
                this._premiumFrame.SetActive(value);
        }

        public TextMeshProUGUI OtherPlayerNickname =>
            this.otherPlayerNickname;

        public GameObject AddToFriendRow =>
            this.addToFriendRow;

        public GameObject FriendRequestRow =>
            this.friendRequestRow;

        public GameObject RemoveFriendRow =>
            this.removeFriendRow;

        public GameObject RevokeFriendRow =>
            this.revokeFriendRow;

        public GameObject EnterBattleAsSpectatorRow =>
            this.enterBattleAsSpectatorRow;
    }
}

