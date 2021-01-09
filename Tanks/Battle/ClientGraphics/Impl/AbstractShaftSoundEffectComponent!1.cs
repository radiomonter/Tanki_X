namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public abstract class AbstractShaftSoundEffectComponent<T> : MonoBehaviour, Component where T: Component
    {
        [SerializeField]
        private GameObject shaftSoundEffectAsset;
        protected T soundComponent;

        protected AbstractShaftSoundEffectComponent()
        {
        }

        public void Init(Transform soundRoot)
        {
            GameObject obj3 = Instantiate<GameObject>(this.shaftSoundEffectAsset);
            Transform transform = obj3.transform;
            transform.parent = soundRoot;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            this.soundComponent = obj3.GetComponent<T>();
        }

        public abstract void Play();
        public abstract void Stop();
    }
}

