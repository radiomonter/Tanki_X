namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using UnityEngine;

    public class ScoreTableRowColorComponent : MonoBehaviour, Component
    {
        public Color rowColor;
        public Color selfRowColor;
        public Color friendRowColor;
    }
}

