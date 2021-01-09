namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class TemperatureEffect : MonoBehaviour
    {
        [SerializeField]
        private Gradient particlesStartColor;
        [SerializeField]
        private AnimationCurve lightIntensity;
        private ParticleSystem particleSystem;

        private void Awake()
        {
            this.particleSystem = base.GetComponent<ParticleSystem>();
        }

        public void SetTemperature(float temperature)
        {
            temperature = Math.Abs(temperature);
            this.particleSystem.startColor = this.particlesStartColor.Evaluate(temperature);
        }
    }
}

