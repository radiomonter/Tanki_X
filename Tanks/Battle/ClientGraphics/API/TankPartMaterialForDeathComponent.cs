namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using UnityEngine;

    public class TankPartMaterialForDeathComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private Material[] deathMaterials;

        public Material[] DeathMaterials =>
            this.deathMaterials;
    }
}

