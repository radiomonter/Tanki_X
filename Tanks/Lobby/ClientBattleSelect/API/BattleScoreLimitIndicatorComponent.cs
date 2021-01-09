namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class BattleScoreLimitIndicatorComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private Text scoreLimitText;

        public int ScoreLimit
        {
            get => 
                int.Parse(this.scoreLimitText.text);
            set => 
                this.scoreLimitText.text = value.ToString();
        }
    }
}

