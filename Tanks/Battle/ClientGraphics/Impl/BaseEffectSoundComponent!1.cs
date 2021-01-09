namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class BaseEffectSoundComponent<T> : MonoBehaviour, Component where T: Component
    {
        [SerializeField]
        private GameObject startSoundAsset;
        [SerializeField]
        private GameObject stopSoundAsset;

        protected BaseEffectSoundComponent()
        {
        }

        public abstract void BeginEffect();
        public void Init(Transform root)
        {
            this.StartSound = this.Init(this.StartSoundAsset, root);
            this.StopSound = this.Init(this.StopSoundAsset, root);
        }

        private T Init(GameObject go, Transform root)
        {
            GameObject obj2 = Instantiate<GameObject>(go);
            obj2.transform.parent = root;
            obj2.transform.localPosition = Vector3.zero;
            obj2.transform.localRotation = Quaternion.identity;
            return obj2.GetComponent<T>();
        }

        public abstract void StopEffect();

        public GameObject StartSoundAsset
        {
            get => 
                this.startSoundAsset;
            set => 
                this.startSoundAsset = value;
        }

        public GameObject StopSoundAsset
        {
            get => 
                this.stopSoundAsset;
            set => 
                this.stopSoundAsset = value;
        }

        public T StartSound { get; set; }

        public T StopSound { get; set; }
    }
}

