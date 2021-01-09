namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using UnityEngine;

    public class TankOutlineEffectBlastEffectUnityComponent : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem blastParticleSystem;

        public void StartEffect()
        {
            this.blastParticleSystem.Play();
        }

        public void StopEffect()
        {
            this.blastParticleSystem.Stop();
        }
    }
}

