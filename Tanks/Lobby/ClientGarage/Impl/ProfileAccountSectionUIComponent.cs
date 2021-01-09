namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class ProfileAccountSectionUIComponent : BehaviourComponent
    {
        public LocalizedField UnconfirmedLocalization;
        [SerializeField]
        private TextMeshProUGUI userChangeNickname;
        [SerializeField]
        private Color emailColor = Color.green;
        [SerializeField]
        private int emailMessageSize = 0x12;
        [SerializeField]
        private Toggle subscribeCheckbox;
        [SerializeField]
        private UserEmailUIComponent userEmail;

        public void SetEmail(string format, string email, string unconfirmedEmail)
        {
            this.userEmail.FormatText = format;
            this.userEmail.UnconfirmedEmail = unconfirmedEmail;
            this.userEmail.Email = email;
        }

        public Color EmailColor =>
            this.emailColor;

        public int EmailMessageSize =>
            this.emailMessageSize;

        public string UserNickname
        {
            get => 
                this.userChangeNickname.text;
            set => 
                this.userChangeNickname.text = value;
        }

        public virtual bool Subscribe
        {
            get => 
                this.subscribeCheckbox.isOn;
            set => 
                this.subscribeCheckbox.isOn = value;
        }
    }
}

