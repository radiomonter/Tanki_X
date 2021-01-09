namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class PersonalXCrystalBonusUIComponent : LocalizedControl, Component
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

