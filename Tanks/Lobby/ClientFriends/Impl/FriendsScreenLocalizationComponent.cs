namespace Tanks.Lobby.ClientFriends.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class FriendsScreenLocalizationComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private Text emptyListNotificationText;
        [SerializeField]
        private Text acceptedFriendHeader;
        [SerializeField]
        private Text possibleFriendHeader;
        [SerializeField]
        private Text searchButtonText;
        [SerializeField]
        private Text searchUserHint;
        [SerializeField]
        private Text searchUserError;
        [SerializeField]
        private Text profileButtonText;
        [SerializeField]
        private Text spectateButtonText;
        [SerializeField]
        private Text acceptButtonText;
        [SerializeField]
        private Text rejectButtonText;
        [SerializeField]
        private Text revokeButtonText;
        [SerializeField]
        private Text removeButtonText;
        [SerializeField]
        private Text removeButtonConfirmText;
        [SerializeField]
        private Text removeButtonCancelText;

        public string EmptyListNotificationText
        {
            set => 
                this.emptyListNotificationText.text = value;
        }

        public string AcceptedFriendHeader
        {
            set => 
                this.acceptedFriendHeader.text = value;
        }

        public string PossibleFriendHeader
        {
            set => 
                this.possibleFriendHeader.text = value;
        }

        public string SearchButtonText
        {
            set => 
                this.searchButtonText.text = value;
        }

        public string SearchUserHint
        {
            set => 
                this.searchUserHint.text = value;
        }

        public string SearchUserError
        {
            set => 
                this.searchUserError.text = value;
        }

        public string ProfileButtonText
        {
            set => 
                this.profileButtonText.text = value;
        }

        public string SpectateButtonText
        {
            set => 
                this.spectateButtonText.text = value;
        }

        public string AcceptButtonText
        {
            set => 
                this.acceptButtonText.text = value;
        }

        public string RejectButtonText
        {
            set => 
                this.rejectButtonText.text = value;
        }

        public string RevokeButtonText
        {
            set => 
                this.revokeButtonText.text = value;
        }

        public string RemoveButtonText
        {
            set => 
                this.removeButtonText.text = value;
        }

        public string RemoveButtonConfirmText
        {
            set => 
                this.removeButtonConfirmText.text = value;
        }

        public string RemoveButtonCancelText
        {
            set => 
                this.removeButtonCancelText.text = value;
        }
    }
}

