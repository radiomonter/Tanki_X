namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ParticleSystemEffectsComponent : BehaviourComponent
    {
        [SerializeField]
        private List<ParticleSystem> particleSystems;

        public void StartEmission()
        {
            foreach (ParticleSystem system in this.particleSystems)
            {
                system.Play();
            }
        }

        public void StopEmission()
        {
            foreach (ParticleSystem system in this.particleSystems)
            {
                system.Stop();
            }
        }
    }
}

