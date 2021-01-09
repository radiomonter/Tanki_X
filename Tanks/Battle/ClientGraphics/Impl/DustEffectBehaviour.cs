namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class DustEffectBehaviour : MonoBehaviour
    {
        public SurfaceType surface;
        public ParticleSystem particleSystem;
        public RandomRange movementSpeedThreshold;
        public RandomRange movementEmissionRate;
        public RandomRange collisionEmissionRate;
        public RandomRange particleLifetime;
        public RandomRange particleSpeed;
        public RandomRange particleSize;
        public RandomRange particleRotation;
        public RandomColor particleColor;
        public float inheritSpeed;
        public float landingCompressionThreshold;

        private ParticleSystem.Particle GetParticle(Vector3 point, Vector3 velocityVector, Vector3 inheritedVelocity, float scale)
        {
            ParticleSystem.Particle particle = new ParticleSystem.Particle {
                randomSeed = (uint) (Random.value * 4.294967E+09f),
                position = point,
                rotation = this.particleRotation.RandomValue,
                velocity = ((velocityVector * this.particleSpeed.RandomValue) * scale) + (inheritedVelocity * this.inheritSpeed),
                size = this.particleSize.RandomValue * scale,
                startLifetime = this.particleLifetime.RandomValue,
                color = this.particleColor.RandomValue * new Color(1f, 1f, 1f, scale * scale)
            };
            particle.remainingLifetime = particle.startLifetime;
            return particle;
        }

        public void TryEmitParticle(Vector3 point, Vector3 inheritedVelocity)
        {
            float magnitude = inheritedVelocity.magnitude;
            float min = this.movementSpeedThreshold.min;
            if (magnitude > this.movementSpeedThreshold.min)
            {
                float scale = (1f + Mathf.Clamp01((magnitude - min) / (this.movementSpeedThreshold.max - min))) / 2f;
                Vector3 vector2 = new Vector3(Random.Range((float) -0.1f, (float) 0.1f), Random.Range((float) 0.9f, (float) 1f), Random.Range((float) -0.1f, (float) 0.1f));
                Vector3 normalized = vector2.normalized;
                ParticleSystem.Particle particle = this.GetParticle(point, normalized, inheritedVelocity, scale);
                this.particleSystem.Emit(particle);
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct RandomColor
        {
            public Color min;
            public Color max;
            public Color RandomValue =>
                Color.Lerp(this.min, this.max, Random.value);
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct RandomRange
        {
            public float min;
            public float max;
            public float RandomValue =>
                Random.Range(this.min, this.max);
        }

        public enum SurfaceType
        {
            None,
            Soil,
            Sand,
            Grass,
            Metal,
            Concrete
        }
    }
}

