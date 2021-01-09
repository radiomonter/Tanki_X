namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Text.RegularExpressions;
    using TMPro;
    using UnityEngine;

    public class UserEmailUIComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI text;
        private string formatText = string.Empty;
        private string unconfirmedEmail = string.Empty;
        private string email = string.Empty;
        private bool emailIsVisible;

        public string FormatText
        {
            get => 
                this.formatText;
            set => 
                this.formatText = value;
        }

        public string UnconfirmedEmail
        {
            get => 
                this.unconfirmedEmail;
            set => 
                this.unconfirmedEmail = value;
        }

        public string Email
        {
            get => 
                this.email;
            set
            {
                this.email = value;
                this.EmailIsVisible = this.emailIsVisible;
            }
        }

        public bool EmailIsVisible
        {
            get => 
                this.emailIsVisible;
            set
            {
                this.emailIsVisible = value;
                string newValue = !this.emailIsVisible ? Regex.Replace(this.email, "[A-Za-z0-9]", "*") : this.email;
                string str2 = !this.emailIsVisible ? Regex.Replace(this.unconfirmedEmail, "[A-Za-z0-9]", "*") : this.unconfirmedEmail;
                this.text.text = this.formatText.Replace("%EMAIL%", newValue).Replace("%UNCEMAIL%", str2);
            }
        }
    }
}

