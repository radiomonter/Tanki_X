namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class UpdateRankEffectParticleMovement : MonoBehaviour
    {
        public Transform parent;
        private ParticleSystem particleSystem;
        private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[0x3e8];
        private Vector3 previousPosition;
        private Vector3 delta;

        private void LateUpdate()
        {
            int particles = this.particleSystem.GetParticles(this.particles);
            for (int i = 0; i < particles; i++)
            {
                this.particles[i].position += this.delta;
            }
            this.particleSystem.SetParticles(this.particles, particles);
            this.delta = this.parent.position - this.previousPosition;
            this.previousPosition = this.parent.position;
        }

        private void Start()
        {
            this.particleSystem = base.GetComponent<ParticleSystem>();
            this.previousPosition = this.parent.position;
        }
    }
}

