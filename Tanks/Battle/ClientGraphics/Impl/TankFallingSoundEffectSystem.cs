namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class TankFallingSoundEffectSystem : ECSSystem
    {
        private const float DESTROY_OFFSET_SEC = 0.2f;
        private const string UNKNOWN_FALLING_EXCEPTON = "Illegal type of falling";
        private const string NO_FALLING_CLIPS_EXCEPTON = "No audio clips for falling";

        private AudioClip GetFallingAudioClip(TankFallingSoundEffectComponent effect)
        {
            AudioClip[] fallingClips = effect.FallingClips;
            int length = fallingClips.Length;
            if (length == 0)
            {
                throw new ArgumentException("No audio clips for falling");
            }
            AudioClip clip = fallingClips[effect.FallingClipIndex];
            effect.FallingClipIndex++;
            if (effect.FallingClipIndex >= length)
            {
                effect.FallingClipIndex = 0;
            }
            return clip;
        }

        [OnEventFire]
        public void PlayFallingSound(TankFallEvent evt, TankFallingSoundEffectNode tank, [JoinAll] SingleNode<MapDustComponent> map)
        {
            TankFallingSoundEffectComponent tankFallingSoundEffect = tank.tankFallingSoundEffect;
            float minPower = tankFallingSoundEffect.MinPower;
            float maxPower = tankFallingSoundEffect.MaxPower;
            float num4 = Mathf.Clamp01((evt.FallingPower - minPower) / (maxPower - minPower));
            if ((num4 > 0f) && (evt.FallingType != TankFallingType.NOTHING))
            {
                Transform soundRootTransform = tank.tankSoundRoot.SoundRootTransform;
                AudioSource source = this.PrepareAudioSource(evt, tankFallingSoundEffect, map.component, soundRootTransform);
                source.volume = num4;
                source.Play();
            }
        }

        private AudioSource PrepareAudioSource(TankFallEvent evt, TankFallingSoundEffectComponent tankFallingSoundEffect, MapDustComponent mapDust, Transform root)
        {
            AudioSource collisionSourceAsset;
            switch (evt.FallingType)
            {
                case TankFallingType.TANK:
                case TankFallingType.VERTICAL_STATIC:
                    collisionSourceAsset = tankFallingSoundEffect.CollisionSourceAsset;
                    break;

                case TankFallingType.FLAT_STATIC:
                case TankFallingType.SLOPED_STATIC_WITH_TRACKS:
                    collisionSourceAsset = tankFallingSoundEffect.FallingSourceAsset;
                    break;

                case TankFallingType.SLOPED_STATIC_WITH_COLLISION:
                {
                    DustEffectBehaviour effectByTag = mapDust.GetEffectByTag(evt.FallingTransform, Vector2.zero);
                    if (effectByTag == null)
                    {
                        collisionSourceAsset = tankFallingSoundEffect.FallingSourceAsset;
                    }
                    else
                    {
                        DustEffectBehaviour.SurfaceType surface = effectByTag.surface;
                        collisionSourceAsset = ((surface == DustEffectBehaviour.SurfaceType.Metal) || (surface == DustEffectBehaviour.SurfaceType.Concrete)) ? tankFallingSoundEffect.CollisionSourceAsset : tankFallingSoundEffect.FallingSourceAsset;
                    }
                    break;
                }
                default:
                    throw new ArgumentException("Illegal type of falling");
            }
            GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                Prefab = collisionSourceAsset.gameObject,
                AutoRecycleTime = ((collisionSourceAsset != tankFallingSoundEffect.FallingSourceAsset) ? collisionSourceAsset.clip : this.GetFallingAudioClip(tankFallingSoundEffect)).length + 0.2f
            };
            base.ScheduleEvent(eventInstance, new EntityStub());
            Transform instance = eventInstance.Instance;
            AudioSource component = instance.GetComponent<AudioSource>();
            instance.parent = root;
            instance.localPosition = Vector3.zero;
            instance.localRotation = Quaternion.identity;
            instance.gameObject.SetActive(true);
            component.Play();
            return component;
        }

        public class TankFallingSoundEffectNode : Node
        {
            public TankFallingSoundEffectComponent tankFallingSoundEffect;
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankSoundRootComponent tankSoundRoot;
        }
    }
}

