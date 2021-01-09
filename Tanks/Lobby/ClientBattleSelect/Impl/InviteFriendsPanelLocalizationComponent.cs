namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class InviteFriendsPanelLocalizationComponent : BehaviourComponent
    {
        [SerializeField]
        private Text showInviteFriendsPanelButton;
        [SerializeField]
        private Text hideInviteFriendsPanelButton;
        [SerializeField]
        private Text emptyListNotification;

        public string ShowInviteFriendsPanelButton
        {
            set => 
                this.showInviteFriendsPanelButton.text = value;
        }

        public string HideInviteFriendsPanelButton
        {
            set => 
                this.hideInviteFriendsPanelButton.text = value;
        }

        public string EmptyListNotification
        {
            set => 
                this.emptyListNotification.text = value;
        }

        public string InviteSentNotification { get; set; }
    }
}

