namespace Tanks.Battle.ClientGraphics
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using UnityEngine;

    public class TankDeathExplosionPrefabsComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject soundPrefab;
        [SerializeField]
        private ParticleSystem explosionPrefab;
        [SerializeField]
        private ParticleSystem firePrefab;

        public GameObject SoundPrefab =>
            this.soundPrefab;

        public ParticleSystem ExplosionPrefab =>
            this.explosionPrefab;

        public ParticleSystem FirePrefab =>
            this.firePrefab;
    }
}

