namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class ProfileScreenLocalizationComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private Text logoutButtonText;
        [SerializeField]
        private Text logoutButtonConfirmText;
        [SerializeField]
        private Text logoutButtonCancelText;
        [SerializeField]
        private Text requestFriendButtonText;
        [SerializeField]
        private Text revokeFriendButtonText;
        [SerializeField]
        private Text acceptFriendButtonText;
        [SerializeField]
        private Text rejectFriendButtonText;
        [SerializeField]
        private Text removeFriendButtonText;
        [SerializeField]
        private Text removeFriendButtonConfirmText;
        [SerializeField]
        private Text removeFriendButtonCancelText;
        [SerializeField]
        private Text goToConfirmEmailScreenButtonText;
        [SerializeField]
        private Text goToChangeUIDScreenButtonText;
        [SerializeField]
        private Text goToPromoCodesScreenButtonText;
        [SerializeField]
        private Text enterAsSpectatorButtonText;

        public string RequestFriendButtonText
        {
            set => 
                this.requestFriendButtonText.text = value.ToUpper();
        }

        public string RevokeFriendButtonText
        {
            set => 
                this.revokeFriendButtonText.text = value.ToUpper();
        }

        public string AcceptFriendButtonText
        {
            set => 
                this.acceptFriendButtonText.text = value.ToUpper();
        }

        public string RejectFriendButtonText
        {
            set => 
                this.rejectFriendButtonText.text = value.ToUpper();
        }

        public string RemoveFriendButtonText
        {
            set => 
                this.removeFriendButtonText.text = value.ToUpper();
        }

        public string RemoveFriendButtonConfirmText
        {
            set => 
                this.removeFriendButtonConfirmText.text = value.ToUpper();
        }

        public string RemoveFriendButtonCancelText
        {
            set => 
                this.removeFriendButtonCancelText.text = value.ToUpper();
        }

        public string LogoutButtonText
        {
            set => 
                this.logoutButtonText.text = value.ToUpper();
        }

        public string LogoutButtonConfirmText
        {
            set => 
                this.logoutButtonConfirmText.text = value.ToUpper();
        }

        public string LogoutButtonCancelText
        {
            set => 
                this.logoutButtonCancelText.text = value.ToUpper();
        }

        public string GoToConfirmEmailScreenButtonText
        {
            set => 
                this.goToConfirmEmailScreenButtonText.text = value.ToUpper();
        }

        public string GoToChangeUIDScreenButtonText
        {
            set => 
                this.goToChangeUIDScreenButtonText.text = value.ToUpper();
        }

        public string GoToPromoCodesScreenButtonText
        {
            set => 
                this.goToPromoCodesScreenButtonText.text = value.ToUpper();
        }

        public string EnterAsSpectatorButtonText
        {
            set => 
                this.enterAsSpectatorButtonText.text = value.ToUpper();
        }

        public string ProfileHeaderText { get; set; }

        public string MyProfileHeaderText { get; set; }

        public string FriendsProfileHeaderText { get; set; }

        public string OfferFriendShipText { get; set; }

        public string FriendRequestSentText { get; set; }
    }
}

