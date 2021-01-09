namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public abstract class BaseRicochetSoundEffectComponent : BehaviourComponent
    {
        [SerializeField]
        private AudioSource assetSource;
        [SerializeField]
        private float lifetime = 2f;

        protected BaseRicochetSoundEffectComponent()
        {
        }

        public abstract void Play(AudioSource sourceInstance);
        public virtual void PlayEffect(Vector3 position)
        {
            GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                Prefab = this.assetSource.gameObject,
                AutoRecycleTime = this.lifetime
            };
            base.ScheduleEvent(eventInstance, new EntityStub());
            Transform instance = eventInstance.Instance;
            instance.position = position;
            instance.rotation = Quaternion.identity;
            AudioSource component = instance.GetComponent<AudioSource>();
            component.gameObject.SetActive(true);
            this.Play(component);
        }
    }
}

