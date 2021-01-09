namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System.Collections.Generic;
    using UnityEngine;

    public class ScoreTableUserRowIndicatorsComponent : MonoBehaviour, Component
    {
        public List<ScoreTableRowIndicator> indicators = new List<ScoreTableRowIndicator>();
    }
}

