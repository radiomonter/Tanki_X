namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class ChangeNicknameButtonComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI price;
        [SerializeField]
        private PaletteColorField notEnoughColor;

        public bool Enough
        {
            set => 
                this.price.color = !value ? this.notEnoughColor.Color : Color.white;
        }

        public string XPrice
        {
            set => 
                this.price.text = value + "<sprite=9>";
        }
    }
}

