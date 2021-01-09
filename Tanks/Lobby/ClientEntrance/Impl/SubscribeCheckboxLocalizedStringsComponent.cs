namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class SubscribeCheckboxLocalizedStringsComponent : FromConfigBehaviour, Component
    {
        [SerializeField]
        private TextMeshProUGUI subscribeLine1Text;
        [SerializeField]
        private TextMeshProUGUI subscribeLine2Text;

        protected override string GetRelativeConfigPath() => 
            "/ui/element";

        public string SubscribeLine1Text
        {
            set => 
                this.subscribeLine1Text.text = value;
        }

        public string SubscribeLine2Text
        {
            set => 
                this.subscribeLine2Text.text = value;
        }
    }
}

