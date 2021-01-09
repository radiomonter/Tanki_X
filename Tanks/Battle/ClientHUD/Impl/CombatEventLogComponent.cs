namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using UnityEngine;

    public class CombatEventLogComponent : MonoBehaviour, Component
    {
        public Color NeutralColor;
        public Color AllyColor;
        public Color EnemyColor;
        public Color RedTeamColor;
        public Color BlueTeamColor;
    }
}

