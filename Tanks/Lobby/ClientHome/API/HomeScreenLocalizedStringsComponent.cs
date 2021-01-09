namespace Tanks.Lobby.ClientHome.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class HomeScreenLocalizedStringsComponent : BehaviourComponent
    {
        [SerializeField]
        private Text playButtonLabel;
        [SerializeField]
        private Text battlesButtonLabel;
        [SerializeField]
        private Text garageButtonLabel;
        [SerializeField]
        private Text questsButtonLabel;
        [SerializeField]
        private Text containersButtonLabel;

        public string PlayButtonLabel
        {
            set => 
                this.playButtonLabel.text = value;
        }

        public virtual string BattlesButtonLabel
        {
            set => 
                this.battlesButtonLabel.text = value;
        }

        public virtual string GarageButtonLabel
        {
            set => 
                this.garageButtonLabel.text = value;
        }

        public virtual string QuestsButtonLabel
        {
            set => 
                this.questsButtonLabel.text = value;
        }

        public virtual string ContainersButtonLabel
        {
            set => 
                this.containersButtonLabel.text = value;
        }
    }
}

