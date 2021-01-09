namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class ScoreTableKillsIndicatorComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private TextMeshProUGUI killsText;

        public int Kills
        {
            get => 
                int.Parse(this.killsText.text);
            set => 
                this.killsText.text = value.ToString();
        }
    }
}

