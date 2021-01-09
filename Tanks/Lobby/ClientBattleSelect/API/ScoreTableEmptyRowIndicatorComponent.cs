namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class ScoreTableEmptyRowIndicatorComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private UnityEngine.UI.Text text;

        public string Text
        {
            get => 
                this.text.text;
            set => 
                this.text.text = value;
        }
    }
}

