namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class BattleSelectScreenLocalizationComponent : BehaviourComponent
    {
        [SerializeField]
        private Text playButton;
        [SerializeField]
        private Text archivedBattle;
        [SerializeField]
        private Text archivedBattleTeam;
        [SerializeField]
        private Text playRedButton;
        [SerializeField]
        private Text playBlueButton;
        [SerializeField]
        private Text watchButton;
        [SerializeField]
        private Text redTeamName;
        [SerializeField]
        private Text blueTeamName;

        public string PlayButton
        {
            set => 
                this.playButton.text = value;
        }

        public string PlayRedButton
        {
            set => 
                this.playRedButton.text = value;
        }

        public string PlayBlueButton
        {
            set => 
                this.playBlueButton.text = value;
        }

        public string WatchButton
        {
            set => 
                this.watchButton.text = value;
        }

        public string BattleLevelsIndicatorText { get; set; }

        public string LevelWarningEquipDowngradedText { get; set; }

        public string LevelWarningXpReducedText { get; set; }

        public string LevelWarningEquipDowngradedAndXpReducedText { get; set; }

        public string LevelErrorText { get; set; }

        public string ArchivedBattleText
        {
            set
            {
                this.archivedBattle.text = value;
                this.archivedBattleTeam.text = value;
            }
        }

        public string RedTeamName
        {
            set => 
                this.redTeamName.text = value;
        }

        public string BlueTeamName
        {
            set => 
                this.blueTeamName.text = value;
        }
    }
}

