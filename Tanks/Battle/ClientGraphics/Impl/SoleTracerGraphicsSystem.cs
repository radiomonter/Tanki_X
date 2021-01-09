namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class SoleTracerGraphicsSystem : ECSSystem
    {
        [OnEventFire]
        public void Init(NodeAddedEvent evt, SoleTracerGraphicsInitNode node)
        {
            MuzzlePointComponent muzzlePoint = node.muzzlePoint;
            SoleTracerGraphicsComponent soleTracerGraphics = node.soleTracerGraphics;
            soleTracerGraphics.Tracer = Object.Instantiate<ParticleSystem>(soleTracerGraphics.Tracer);
            UnityUtil.InheritAndEmplace(soleTracerGraphics.Tracer.transform, muzzlePoint.Current);
            node.Entity.AddComponent<SoleTracerGraphicsReadyComponent>();
        }

        [OnEventFire]
        public unsafe void InstantiatePellets(BaseShotEvent evt, SoleTracerGraphicsNode weapon)
        {
            RaycastHit hit;
            SoleTracerGraphicsComponent soleTracerGraphics = weapon.soleTracerGraphics;
            float radiusOfMinDamage = weapon.damageWeakeningByDistance.RadiusOfMinDamage;
            float num2 = radiusOfMinDamage / soleTracerGraphics.Tracer.startSpeed;
            ParticleSystem.Particle particle = new ParticleSystem.Particle {
                position = soleTracerGraphics.Tracer.transform.position,
                color = soleTracerGraphics.Tracer.startColor,
                size = soleTracerGraphics.Tracer.startSize
            };
            particle.randomSeed = (uint) (Random.value * 4.294967E+09f);
            particle.velocity = evt.ShotDirection * soleTracerGraphics.Tracer.startSpeed;
            particle.startLifetime = !Physics.Raycast(soleTracerGraphics.Tracer.transform.position, evt.ShotDirection, out hit, radiusOfMinDamage, LayerMasks.GUN_TARGETING_WITH_DEAD_UNITS) ? num2 : (Vector3.Distance(soleTracerGraphics.Tracer.transform.position, hit.point) / soleTracerGraphics.Tracer.startSpeed);
            if (particle.startLifetime > soleTracerGraphics.MaxTime)
            {
                ParticleSystem.Particle* particlePtr1 = &particle;
                particlePtr1.velocity *= particle.startLifetime / soleTracerGraphics.MaxTime;
                particle.startLifetime = soleTracerGraphics.MaxTime;
            }
            particle.remainingLifetime = particle.startLifetime;
            soleTracerGraphics.Tracer.Emit(particle);
        }

        public class SoleTracerGraphicsInitNode : Node
        {
            public MuzzlePointComponent muzzlePoint;
            public SoleTracerGraphicsComponent soleTracerGraphics;
        }

        public class SoleTracerGraphicsNode : Node
        {
            public SoleTracerGraphicsReadyComponent soleTracerGraphicsReady;
            public SoleTracerGraphicsComponent soleTracerGraphics;
            public MuzzlePointComponent muzzlePoint;
            public DamageWeakeningByDistanceComponent damageWeakeningByDistance;
            public WeaponUnblockedComponent weaponUnblocked;
        }
    }
}

