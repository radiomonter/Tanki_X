namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class FractionScoresUiBehaviour : MonoBehaviour
    {
        [SerializeField]
        private ImageSkin _fractionLogo;
        [SerializeField]
        private TextMeshProUGUI _fractionName;
        [SerializeField]
        private TextMeshProUGUI _fractionScores;
        private long _scores;
        [SerializeField]
        private GameObject _winnerMark;

        public string FractionLogoUid
        {
            set => 
                this._fractionLogo.SpriteUid = value;
        }

        public string FractionName
        {
            set => 
                this._fractionName.text = value;
        }

        public Color FractionColor
        {
            set => 
                this._fractionName.color = value;
        }

        public long FractionScores
        {
            get => 
                this._scores;
            set
            {
                this._fractionScores.text = value.ToString();
                this._scores = value;
            }
        }

        public bool IsWinner
        {
            set => 
                this._winnerMark.SetActive(value);
        }
    }
}

