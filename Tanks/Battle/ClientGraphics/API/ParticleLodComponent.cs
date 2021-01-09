namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class ParticleLodComponent : MonoBehaviour
    {
        public float[] coefficient;

        private void Start()
        {
            int currentParticleQuality = GraphicsSettings.INSTANCE.CurrentParticleQuality;
            float num2 = this.coefficient[currentParticleQuality];
            ParticleSystem[] componentsInChildren = base.GetComponentsInChildren<ParticleSystem>();
            int index = 0;
            while (index < componentsInChildren.Length)
            {
                ParticleSystem system = componentsInChildren[index];
                ParticleSystem.MainModule main = system.main;
                main.maxParticles = Mathf.Max(Mathf.Min(main.maxParticles, 1), (int) (main.maxParticles * num2));
                ParticleSystem.EmissionModule emission = system.emission;
                emission.rateOverTimeMultiplier = Mathf.Max(Mathf.Min(emission.rateOverTimeMultiplier, 1f), (float) ((int) (emission.rateOverTimeMultiplier * num2)));
                emission.rateOverDistanceMultiplier = Mathf.Max(Mathf.Min(emission.rateOverDistanceMultiplier, 1f), (float) ((int) (emission.rateOverDistanceMultiplier * num2)));
                ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[emission.burstCount];
                emission.GetBursts(bursts);
                int num4 = 0;
                while (true)
                {
                    if (num4 >= emission.burstCount)
                    {
                        emission.SetBursts(bursts);
                        index++;
                        break;
                    }
                    bursts[num4].minCount = (short) Mathf.Max((float) Mathf.Min(bursts[num4].minCount, 1), bursts[num4].minCount * num2);
                    bursts[num4].maxCount = (short) Mathf.Max((float) Mathf.Min(bursts[num4].maxCount, 1), bursts[num4].maxCount * num2);
                    num4++;
                }
            }
        }
    }
}

