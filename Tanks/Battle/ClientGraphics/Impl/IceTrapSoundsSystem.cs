namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class IceTrapSoundsSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Action<IceTrapExplosionSoundBehaviour> <>f__am$cache0;
        [CompilerGenerated]
        private static Action<SoundFadeBehaviour> <>f__am$cache1;

        [OnEventComplete]
        public void PlayExplosionSound(MineExplosionEvent e, IceTrapNode effect, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            Transform transform = effect.effectInstance.GameObject.transform;
            GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                Prefab = effect.iceTrapExplosionSound.ExplosionSoundAsset,
                AutoRecycleTime = effect.iceTrapExplosionSound.Lifetime
            };
            base.ScheduleEvent(eventInstance, effect);
            Transform instance = eventInstance.Instance;
            GameObject gameObject = instance.gameObject;
            instance.position = transform.position;
            instance.rotation = transform.rotation;
            gameObject.SetActive(true);
            Object.DontDestroyOnLoad(gameObject);
        }

        [OnEventFire]
        public void PrepareCleaningForEffects(NodeRemoveEvent evt, SingleNode<SelfBattleUserComponent> battleUser)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate (IceTrapExplosionSoundBehaviour i) {
                    if (<>f__am$cache1 == null)
                    {
                        <>f__am$cache1 = delegate (SoundFadeBehaviour s) {
                            s.enabled = true;
                            RFX4_AudioCurves component = s.GetComponent<RFX4_AudioCurves>();
                            RFX4_StartDelay delay = s.GetComponent<RFX4_StartDelay>();
                            if (component)
                            {
                                Object.Destroy(component);
                            }
                            if (delay)
                            {
                                Object.Destroy(delay);
                            }
                        };
                    }
                    i.GetComponentsInChildren<SoundFadeBehaviour>(true).ForEach<SoundFadeBehaviour>(<>f__am$cache1);
                };
            }
            Object.FindObjectsOfType<IceTrapExplosionSoundBehaviour>().ForEach<IceTrapExplosionSoundBehaviour>(<>f__am$cache0);
        }

        public class IceTrapNode : Node
        {
            public IcetrapEffectComponent icetrapEffect;
            public IceTrapExplosionSoundComponent iceTrapExplosionSound;
            public EffectInstanceComponent effectInstance;
        }
    }
}

