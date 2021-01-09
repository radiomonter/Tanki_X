namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class ParticleSystemAnimationBehaviour : MonoBehaviour
    {
        public ParticleSystem particleSystem;

        public void PlayParticleSystem()
        {
            this.particleSystem.Play();
        }
    }
}

