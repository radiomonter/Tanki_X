namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class TankFrictionSoundSystem : ECSSystem
    {
        [OnEventFire]
        public void DisableTankFrictionSound(NodeAddedEvent evt, ReadyDeadTankFrictionSoundNode tank)
        {
            this.StopSounds(tank.tankFrictionSoundEffect);
            tank.tankFrictionSoundEffectReady.TankFrictionCollideSoundBehaviour.enabled = false;
            tank.tankFrictionSoundEffectReady.TankFrictionSoundBehaviour.enabled = false;
        }

        [OnEventFire]
        public void EnableTankFrictionSound(NodeAddedEvent evt, ReadyActiveTankFrictionSoundNode tank)
        {
            tank.tankFrictionSoundEffectReady.TankFrictionCollideSoundBehaviour.enabled = true;
            tank.tankFrictionSoundEffectReady.TankFrictionSoundBehaviour.enabled = true;
        }

        private SoundController InitSoundTransforms(SoundController sourcePrefab, Transform root)
        {
            SoundController controller = Object.Instantiate<SoundController>(sourcePrefab);
            Transform transform = controller.transform;
            transform.parent = root;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            return controller;
        }

        [OnEventFire]
        public void InitTankFrictionSound(NodeAddedEvent evt, [Combine] InitialTankFrictionSoundNode tank, SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            Transform soundRootTransform = tank.tankSoundRoot.SoundRootTransform;
            TankFrictionSoundEffectComponent tankFrictionSoundEffect = tank.tankFrictionSoundEffect;
            SoundController metallFrictionSourcePrefab = tankFrictionSoundEffect.MetallFrictionSourcePrefab;
            SoundController stoneFrictionSourcePrefab = tankFrictionSoundEffect.StoneFrictionSourcePrefab;
            SoundController frictionContactSourcePrefab = tankFrictionSoundEffect.FrictionContactSourcePrefab;
            tankFrictionSoundEffect.MetallFrictionSourceInstance = this.InitSoundTransforms(metallFrictionSourcePrefab, soundRootTransform);
            tankFrictionSoundEffect.StoneFrictionSourceInstance = this.InitSoundTransforms(stoneFrictionSourcePrefab, soundRootTransform);
            tankFrictionSoundEffect.FrictionContactSourceInstance = this.InitSoundTransforms(frictionContactSourcePrefab, soundRootTransform);
            GameObject gameObject = tank.tankFrictionMarker.gameObject;
            TankFrictionSoundBehaviour behaviour = gameObject.AddComponent<TankFrictionSoundBehaviour>();
            gameObject.layer = Layers.FRICTION;
            behaviour.Init(tank.Entity);
            float halfLength = tank.tankColliders.BoundsCollider.bounds.size.z * 0.5f;
            TankFrictionCollideSoundBehaviour behaviour2 = tank.tankCollision.gameObject.AddComponent<TankFrictionCollideSoundBehaviour>();
            behaviour2.Init(tank.tankFrictionSoundEffect.FrictionContactSourceInstance, tank.rigidbody.Rigidbody, halfLength, tank.tankFallingSoundEffect.MinPower, tank.tankFallingSoundEffect.MaxPower);
            TankFrictionSoundEffectReadyComponent component = new TankFrictionSoundEffectReadyComponent {
                TankFrictionCollideSoundBehaviour = behaviour2,
                TankFrictionSoundBehaviour = behaviour
            };
            tank.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void PlayTankFrictionSound(UpdateEvent evt, ReadyActiveTankFrictionSoundNode tank)
        {
            TankFrictionSoundBehaviour tankFrictionSoundBehaviour = tank.tankFrictionSoundEffectReady.TankFrictionSoundBehaviour;
            if (tankFrictionSoundBehaviour.TriggerStay)
            {
                TankFrictionSoundEffectComponent tankFrictionSoundEffect = tank.tankFrictionSoundEffect;
                Collider frictionCollider = tankFrictionSoundBehaviour.FrictionCollider;
                if (frictionCollider == null)
                {
                    this.StopSounds(tankFrictionSoundEffect);
                }
                else
                {
                    Vector3 velocity = tank.rigidbody.Rigidbody.velocity;
                    if (frictionCollider.gameObject.layer == Layers.FRICTION)
                    {
                        this.SetMetallFriction(tankFrictionSoundEffect, velocity);
                    }
                    else
                    {
                        DustEffectBehaviour effect = tank.collisionDust.CollisionDustBehaviour.Effect;
                        if (effect == null)
                        {
                            this.StopSounds(tankFrictionSoundEffect);
                        }
                        else
                        {
                            DustEffectBehaviour.SurfaceType surface = effect.surface;
                            if (surface == DustEffectBehaviour.SurfaceType.Metal)
                            {
                                this.SetMetallFriction(tankFrictionSoundEffect, velocity);
                            }
                            else if (surface != DustEffectBehaviour.SurfaceType.Concrete)
                            {
                                this.StopSounds(tankFrictionSoundEffect);
                            }
                            else
                            {
                                this.SetStoneFriction(tankFrictionSoundEffect, velocity);
                            }
                        }
                    }
                }
            }
        }

        private void SetFriction(SoundController actualSource, SoundController stopSource, TankFrictionSoundEffectComponent tankFrictionSoundEffect, Vector3 velocity)
        {
            this.SetFrictionVolume(actualSource, tankFrictionSoundEffect, velocity);
            actualSource.FadeIn();
            stopSource.FadeOut();
        }

        private void SetFrictionVolume(SoundController sound, TankFrictionSoundEffectComponent tankFrictionSoundEffect, Vector3 velocity)
        {
            float minValuableFrictionPower = tankFrictionSoundEffect.MinValuableFrictionPower;
            this.SetVolumeByVelocity(sound, velocity, minValuableFrictionPower, tankFrictionSoundEffect.MaxValuableFrictionPower);
        }

        private void SetMetallFriction(TankFrictionSoundEffectComponent tankFrictionSoundEffect, Vector3 velocity)
        {
            this.SetFriction(tankFrictionSoundEffect.MetallFrictionSourceInstance, tankFrictionSoundEffect.StoneFrictionSourceInstance, tankFrictionSoundEffect, velocity);
        }

        private void SetStoneFriction(TankFrictionSoundEffectComponent tankFrictionSoundEffect, Vector3 velocity)
        {
            this.SetFriction(tankFrictionSoundEffect.StoneFrictionSourceInstance, tankFrictionSoundEffect.MetallFrictionSourceInstance, tankFrictionSoundEffect, velocity);
        }

        private void SetVolumeByVelocity(SoundController sound, Vector3 velocity, float min, float max)
        {
            if (sound != null)
            {
                float num2 = Mathf.Clamp01((velocity.sqrMagnitude - min) / (max - min));
                sound.MaxVolume = num2;
            }
        }

        private void StopSounds(TankFrictionSoundEffectComponent tankFrictionSoundEffect)
        {
            tankFrictionSoundEffect.MetallFrictionSourceInstance.FadeOut();
            tankFrictionSoundEffect.StoneFrictionSourceInstance.FadeOut();
        }

        [OnEventFire]
        public void StopTankFrictionSound(TankFrictionExitEvent evt, ReadyTankFrictionSoundNode tank)
        {
            TankFrictionSoundEffectComponent tankFrictionSoundEffect = tank.tankFrictionSoundEffect;
            this.StopSounds(tankFrictionSoundEffect);
        }

        public class InitialTankFrictionSoundNode : Node
        {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankCollidersComponent tankColliders;
            public TankFrictionMarkerComponent tankFrictionMarker;
            public TankFrictionSoundEffectComponent tankFrictionSoundEffect;
            public TankFallingSoundEffectComponent tankFallingSoundEffect;
            public TankCollisionComponent tankCollision;
            public TankSoundRootComponent tankSoundRoot;
            public CollisionDustComponent collisionDust;
            public RigidbodyComponent rigidbody;
        }

        public class ReadyActiveTankFrictionSoundNode : TankFrictionSoundSystem.ReadyTankFrictionSoundNode
        {
            public TankActiveStateComponent tankActiveState;
        }

        public class ReadyDeadTankFrictionSoundNode : TankFrictionSoundSystem.ReadyTankFrictionSoundNode
        {
            public TankDeadStateComponent tankDeadState;
        }

        public class ReadyTankFrictionSoundNode : TankFrictionSoundSystem.InitialTankFrictionSoundNode
        {
            public TankFrictionSoundEffectReadyComponent tankFrictionSoundEffectReady;
        }
    }
}

