namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientGarage.Impl;
    using TMPro;
    using UnityEngine;

    public class PersonalDiscountUIComponent : TextTimerComponent, Component
    {
        [SerializeField]
        private TextMeshProUGUI description;

        public string Description
        {
            set => 
                this.description.text = value;
        }
    }
}

