namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class PhoneInputComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI code;

        public void SetCode(string code)
        {
            this.code.text = code;
        }
    }
}

