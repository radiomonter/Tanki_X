namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class PresetLabelComponent : BehaviourComponent
    {
        private TextMeshProUGUI text =>
            base.GetComponent<TextMeshProUGUI>();

        public string PresetName
        {
            get => 
                this.text.text;
            set => 
                this.text.text = value;
        }
    }
}

