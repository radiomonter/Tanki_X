namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using UnityEngine;

    public class NameplateTeamColorComponent : MonoBehaviour, Component
    {
        public Color redTeamColor;
        public Color blueTeamColor;
        public Color dmColor;
    }
}

