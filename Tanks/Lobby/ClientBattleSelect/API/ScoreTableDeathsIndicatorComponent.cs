namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class ScoreTableDeathsIndicatorComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private TextMeshProUGUI deathsText;

        public int Deaths
        {
            get => 
                int.Parse(this.deathsText.text);
            set => 
                this.deathsText.text = value.ToString();
        }
    }
}

