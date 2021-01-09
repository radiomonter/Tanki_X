namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class HelpOverlayTextComponent : LocalizedControl
    {
        [SerializeField]
        private Text controlsText;
        [SerializeField]
        private Text turretText;
        [SerializeField]
        private Text fireText;
        [SerializeField]
        private Text spaceText;
        [SerializeField]
        private Text orText;
        [SerializeField]
        private Text additionalControlsText;
        [SerializeField]
        private Text modulesText;
        [SerializeField]
        private Text graffitiText;
        [SerializeField]
        private Text selfDestructText;
        [SerializeField]
        private Text helpText;
        [SerializeField]
        private Text exitText;

        public string ControlsText
        {
            set => 
                this.controlsText.text = value;
        }

        public string TurretText
        {
            set => 
                this.turretText.text = value;
        }

        public string FireText
        {
            set => 
                this.fireText.text = value;
        }

        public string SpaceText
        {
            set => 
                this.spaceText.text = value;
        }

        public string OrText
        {
            set => 
                this.orText.text = value;
        }

        public string AdditionalControlsText
        {
            set => 
                this.additionalControlsText.text = value;
        }

        public string ModulesText
        {
            set => 
                this.modulesText.text = value;
        }

        public string GraffitiText
        {
            set => 
                this.graffitiText.text = value;
        }

        public string SelfDestructText
        {
            set => 
                this.selfDestructText.text = value;
        }

        public string HelpText
        {
            set => 
                this.helpText.text = value;
        }

        public string ExitText
        {
            set => 
                this.exitText.text = value;
        }
    }
}

