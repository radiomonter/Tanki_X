namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class FractionUserScoreUiComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI _scoreText;

        public long Scores
        {
            set => 
                this._scoreText.text = value.ToString();
        }
    }
}

