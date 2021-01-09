namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using TMPro;
    using UnityEngine;

    public class EnterNewPasswordScreenComponent : LocalizedScreenComponent, NoScaleScreen
    {
        [SerializeField]
        private TextMeshProUGUI newPassword;
        [SerializeField]
        private TextMeshProUGUI repeatNewPassword;
        [SerializeField]
        private TextMeshProUGUI continueButton;

        public virtual string NewPassword
        {
            set => 
                this.newPassword.text = value;
        }

        public virtual string RepeatNewPassword
        {
            set => 
                this.repeatNewPassword.text = value;
        }

        public virtual string ContinueButton
        {
            set => 
                this.continueButton.text = value;
        }
    }
}

