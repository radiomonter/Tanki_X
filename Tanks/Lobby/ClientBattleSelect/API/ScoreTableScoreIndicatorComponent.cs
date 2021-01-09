namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class ScoreTableScoreIndicatorComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private TextMeshProUGUI scoreText;

        public int Score
        {
            get => 
                int.Parse(this.scoreText.text);
            set => 
                this.scoreText.text = value.ToString();
        }
    }
}

