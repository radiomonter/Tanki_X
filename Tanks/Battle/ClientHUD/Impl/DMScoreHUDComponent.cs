namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class DMScoreHUDComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI place;
        [SerializeField]
        private TextMeshProUGUI playerScore;
        private int _place;
        private int _players;
        private int _playerScore;
        private int _maxScore;

        private void OnDisable()
        {
            base.gameObject.SetActive(false);
        }

        public void UpdatePlayerPlace()
        {
            this.place.text = $"{this._place}<size=12>/{this._players}</size>";
        }

        public void UpdatePlayerScore()
        {
            this.playerScore.text = $"{this._playerScore}<size=12>/{this._maxScore}</size>";
        }

        public int Place
        {
            get => 
                this._place;
            set
            {
                this._place = value;
                this.UpdatePlayerPlace();
            }
        }

        public int Players
        {
            get => 
                this._players;
            set
            {
                this._players = value;
                this.UpdatePlayerPlace();
            }
        }

        public int PlayerScore
        {
            get => 
                this._playerScore;
            set
            {
                this._playerScore = value;
                this.UpdatePlayerScore();
            }
        }

        public int MaxScore
        {
            get => 
                this._maxScore;
            set
            {
                this._maxScore = value;
                this.UpdatePlayerScore();
            }
        }
    }
}

