namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    [RequireComponent(typeof(ParticleSystem))]
    public class MuzzleFlashAdjustBehaviour : MonoBehaviour
    {
        private const float FAR_FLASH_DISTANCE = 10f;
        [SerializeField]
        private ParticleSystem[] systems;
        private float[] startSizes;
        private float[] startSpeeds;
        private ParticleSystem effect;

        private void Awake()
        {
            this.effect = base.GetComponent<ParticleSystem>();
            this.startSizes = new float[this.systems.Length];
            for (int i = 0; i < this.systems.Length; i++)
            {
                this.startSizes[i] = this.systems[i].startSize;
            }
            this.startSpeeds = new float[this.systems.Length];
            for (int j = 0; j < this.systems.Length; j++)
            {
                this.startSpeeds[j] = this.systems[j].startSpeed;
            }
        }

        private float[] GetSystemsScales()
        {
            RaycastHit hit;
            float positiveInfinity = float.PositiveInfinity;
            Ray ray = new Ray(base.transform.position, base.transform.forward);
            if (Physics.Raycast(ray, out hit, 10f, LayerMasks.VISUAL_TARGETING))
            {
                positiveInfinity = hit.distance;
            }
            float[] numArray = new float[this.systems.Length];
            for (int i = 0; i < this.systems.Length; i++)
            {
                ParticleSystem system = this.systems[i];
                ParticleSystemRenderer component = system.GetComponent<ParticleSystemRenderer>();
                float num3 = system.startSpeed * system.startLifetime;
                num3 = (component.renderMode != ParticleSystemRenderMode.Stretch) ? (num3 + (system.startSize * 0.5f)) : (num3 + ((system.startSize * 0.5f) * Mathf.Abs(component.lengthScale)));
                numArray[i] = Mathf.Clamp01((positiveInfinity - Vector3.Distance(base.transform.position, system.transform.position)) / num3);
            }
            return numArray;
        }

        private void OnDisable()
        {
            this.effect.Stop(true);
        }

        private void OnEnable()
        {
            float[] systemsScales = this.GetSystemsScales();
            for (int i = 0; i < this.systems.Length; i++)
            {
                this.UpdateSystem(this.systems[i], systemsScales[i], this.startSizes[i], this.startSpeeds[i]);
            }
            this.effect.Play(true);
            for (int j = 0; j < this.systems.Length; j++)
            {
                this.UpdateParticles(this.systems[j], systemsScales[j]);
            }
        }

        private void UpdateParticles(ParticleSystem system, float scale)
        {
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[system.particleCount];
            system.GetParticles(particles);
            if (system.simulationSpace == ParticleSystemSimulationSpace.Local)
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    particles[i].position *= scale;
                }
            }
            else
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    particles[i].position = base.transform.position + ((particles[i].position - base.transform.position) * scale);
                }
            }
            system.SetParticles(particles, particles.Length);
        }

        private void UpdateSystem(ParticleSystem system, float scale, float size, float speed)
        {
            system.startSize = (size * (1f + scale)) / 2f;
            system.startSpeed = speed * scale;
            system.startColor *= new Color(1f, 1f, 1f, scale * scale);
        }
    }
}

