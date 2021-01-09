namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System.Collections.Generic;
    using UnityEngine;

    public class ScoreTableEmptyRowIndicatorsComponent : MonoBehaviour, Component
    {
        public List<ScoreTableRowIndicator> indicators = new List<ScoreTableRowIndicator>();
        public Color emptyRowColor;
        public Stack<ScoreTableRowComponent> emptyRows = new Stack<ScoreTableRowComponent>();
    }
}

