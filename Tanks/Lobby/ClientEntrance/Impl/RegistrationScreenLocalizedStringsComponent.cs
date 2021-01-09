namespace Tanks.Lobby.ClientEntrance.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientNavigation.API;
    using TMPro;
    using UnityEngine;

    public class RegistrationScreenLocalizedStringsComponent : LocalizedScreenComponent
    {
        [SerializeField]
        private TextMeshProUGUI finishButtonText;
        [SerializeField]
        private TextMeshProUGUI skipButtonText;
        [SerializeField]
        private TextMeshProUGUI nicknameHintText;
        [SerializeField]
        private TextMeshProUGUI passwordHintText;
        [SerializeField]
        private TextMeshProUGUI repeatPasswordHintText;
        [SerializeField]
        private TextMeshProUGUI iAcceptTermsPart1Text;
        [SerializeField]
        private TextMeshProUGUI iAcceptTermsPart2EULAText;
        [SerializeField]
        private TextMeshProUGUI iAcceptTermsPart4RulesText;
        [SerializeField]
        private TextMeshProUGUI iAcceptTermsPart5PrivacyPolicyText;

        public string Finish
        {
            set => 
                this.finishButtonText.text = value;
        }

        public string Skip
        {
            set => 
                this.skipButtonText.text = value;
        }

        public string Nickname
        {
            set => 
                this.nicknameHintText.text = value;
        }

        public string Password
        {
            set => 
                this.passwordHintText.text = value;
        }

        public string RepeatPassword
        {
            set => 
                this.repeatPasswordHintText.text = value;
        }

        public string IAcceptTermsPart1
        {
            set => 
                this.iAcceptTermsPart1Text.text = value;
        }

        public string IAcceptTermsPart2EULA
        {
            set => 
                this.iAcceptTermsPart2EULAText.text = value;
        }

        public string IAcceptTermsPart4Rules
        {
            set => 
                this.iAcceptTermsPart4RulesText.text = value;
        }

        public string IAcceptTermsPart5PrivacyPolicy
        {
            set => 
                this.iAcceptTermsPart5PrivacyPolicyText.text = value;
        }

        public string LicenseAgreementUrl { get; set; }

        public string GameRulesUrl { get; set; }

        public string PrivacyPolicyUrl { get; set; }
    }
}

