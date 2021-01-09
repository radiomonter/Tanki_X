namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class IceTrapExplosionSoundComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject explosionSoundAsset;
        [SerializeField]
        private float lifetime = 7f;

        public float Lifetime =>
            this.lifetime;

        public GameObject ExplosionSoundAsset =>
            this.explosionSoundAsset;
    }
}

