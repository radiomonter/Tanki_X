namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class OpenSelectCountryButtonComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI buttonTitle;
        [SerializeField]
        private LocalizedField country;

        public string CountryCode
        {
            set => 
                this.buttonTitle.text = this.country.Value + ": " + value;
        }
    }
}

