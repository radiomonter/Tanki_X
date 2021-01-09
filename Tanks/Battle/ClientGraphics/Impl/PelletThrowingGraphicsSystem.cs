namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class PelletThrowingGraphicsSystem : ECSSystem
    {
        [OnEventFire]
        public void Init(NodeAddedEvent evt, PelletThrowingGraphicsInitNode node)
        {
            MuzzlePointComponent muzzlePoint = node.muzzlePoint;
            PelletThrowingGraphicsComponent pelletThrowingGraphics = node.pelletThrowingGraphics;
            pelletThrowingGraphics.Trails = InstantiateAndInherit(muzzlePoint, pelletThrowingGraphics.Trails);
            pelletThrowingGraphics.Hits = InstantiateAndInherit(muzzlePoint, pelletThrowingGraphics.Hits);
            CustomRenderQueue.SetQueue(pelletThrowingGraphics.gameObject, 0xc4e);
            node.Entity.AddComponent<PelletThrowingGraphicsReadyComponent>();
        }

        private static ParticleSystem InstantiateAndInherit(MuzzlePointComponent muzzlePoint, ParticleSystem prefab)
        {
            ParticleSystem system = Object.Instantiate<ParticleSystem>(prefab);
            UnityUtil.InheritAndEmplace(system.transform, muzzlePoint.Current);
            return system;
        }

        [OnEventFire]
        public void InstantiatePellets(RemoteHammerShotEvent evt, PelletThrowingGraphicsNode weapon)
        {
            weapon.hammerPelletCone.ShotSeed = evt.RandomSeed;
            InstantiatePelletsByBaseEvent(evt.ShotDirection, weapon);
        }

        [OnEventFire]
        public void InstantiatePellets(SelfHammerShotEvent evt, PelletThrowingGraphicsNode weapon)
        {
            InstantiatePelletsByBaseEvent(evt.ShotDirection, weapon);
        }

        private static unsafe void InstantiatePelletsByBaseEvent(Vector3 shotDirection, PelletThrowingGraphicsNode weapon)
        {
            MuzzlePointComponent muzzlePoint = weapon.muzzlePoint;
            PelletThrowingGraphicsComponent pelletThrowingGraphics = weapon.pelletThrowingGraphics;
            float radiusOfMinDamage = weapon.damageWeakeningByDistance.RadiusOfMinDamage;
            float constant = pelletThrowingGraphics.Trails.main.startSpeed.constant;
            float num3 = radiusOfMinDamage / constant;
            ParticleSystem.EmitParams params2 = new ParticleSystem.EmitParams {
                position = pelletThrowingGraphics.Trails.transform.position,
                startColor = pelletThrowingGraphics.Trails.main.startColor.color,
                startSize = pelletThrowingGraphics.Trails.main.startSizeMultiplier
            };
            ParticleSystem.EmitParams emitParams = params2;
            params2 = new ParticleSystem.EmitParams {
                startColor = pelletThrowingGraphics.Hits.main.startColor.color,
                startSize = pelletThrowingGraphics.Hits.main.startSizeMultiplier
            };
            ParticleSystem.EmitParams params3 = params2;
            Vector3 localDirection = muzzlePoint.Current.InverseTransformVector(shotDirection);
            foreach (Vector3 vector2 in PelletDirectionsCalculator.GetRandomDirections(weapon.hammerPelletCone, muzzlePoint.Current.rotation, localDirection))
            {
                RaycastHit hit;
                ParticleSystem.MainModule main = pelletThrowingGraphics.Trails.main;
                ParticleSystem.MinMaxGradient startColor = main.startColor;
                if (startColor.mode == ParticleSystemGradientMode.RandomColor)
                {
                    emitParams.startColor = pelletThrowingGraphics.Trails.main.startColor.Evaluate(Random.Range((float) 0f, (float) 1f));
                }
                emitParams.randomSeed = (uint) (Random.value * 4.294967E+09f);
                emitParams.velocity = vector2 * constant;
                if (!Physics.Raycast(pelletThrowingGraphics.Trails.transform.position, vector2, out hit, radiusOfMinDamage, LayerMasks.GUN_TARGETING_WITH_DEAD_UNITS))
                {
                    emitParams.startLifetime = num3;
                }
                else
                {
                    emitParams.startLifetime = Vector3.Distance(pelletThrowingGraphics.Trails.transform.position, hit.point) / constant;
                    params3.startLifetime = Random.Range(pelletThrowingGraphics.SparklesMinLifetime, pelletThrowingGraphics.SparklesMaxLifetime);
                    params3.randomSeed = (uint) (Random.value * 4.294967E+09f);
                    params3.position = hit.point;
                    params3.velocity = Random.onUnitSphere;
                    ParticleSystem.EmitParams* paramsPtr1 = &params3;
                    paramsPtr1.velocity *= Mathf.Sign(Vector3.Dot(params3.velocity, hit.normal)) * pelletThrowingGraphics.HitReflectVeolcity;
                    pelletThrowingGraphics.Hits.Emit(params3, 1);
                }
                pelletThrowingGraphics.Trails.Emit(emitParams, 1);
            }
        }

        public class PelletThrowingGraphicsInitNode : Node
        {
            public MuzzlePointComponent muzzlePoint;
            public PelletThrowingGraphicsComponent pelletThrowingGraphics;
        }

        public class PelletThrowingGraphicsNode : Node
        {
            public PelletThrowingGraphicsReadyComponent pelletThrowingGraphicsReady;
            public PelletThrowingGraphicsComponent pelletThrowingGraphics;
            public HammerPelletConeComponent hammerPelletCone;
            public MuzzlePointComponent muzzlePoint;
            public DamageWeakeningByDistanceComponent damageWeakeningByDistance;
            public WeaponUnblockedComponent weaponUnblocked;
        }
    }
}

