namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class MobilePhoneInputComponent : BehaviourComponent
    {
        [SerializeField]
        private Text phoneCountryCode;

        public virtual string PhoneCountryCode
        {
            set => 
                this.phoneCountryCode.text = value;
        }
    }
}

