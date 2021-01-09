namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class ParticleSystemUtils
    {
        public static void StartParticleSystem(ParticleSystem particleSystem)
        {
            particleSystem.enableEmission = true;
            Debug.Log("start particle system " + particleSystem.gameObject.name);
        }

        public static void StartParticleSystems(params ParticleSystem[] particleSystems)
        {
            foreach (ParticleSystem system in particleSystems)
            {
                StartParticleSystem(system);
            }
        }

        public static void StopParticleSystem(ParticleSystem particleSystem)
        {
            particleSystem.enableEmission = false;
            Debug.Log("stop particle system " + particleSystem.gameObject.name);
        }

        public static void StopParticleSystems(params ParticleSystem[] particleSystems)
        {
            foreach (ParticleSystem system in particleSystems)
            {
                StopParticleSystem(system);
            }
        }
    }
}

