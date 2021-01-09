namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class StreamEffectComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject effectPrefab;

        public void Init(MuzzlePointComponent muzzlePoint)
        {
            GameObject gameObject = Instantiate<GameObject>(this.effectPrefab);
            UnityUtil.InheritAndEmplace(gameObject.transform, muzzlePoint.Current);
            this.Instance = gameObject.GetComponent<StreamEffectBehaviour>();
            CustomRenderQueue.SetQueue(gameObject, 0xc4e);
        }

        public StreamEffectBehaviour Instance { get; private set; }

        public GameObject EffectPrefab
        {
            get => 
                this.effectPrefab;
            set => 
                this.effectPrefab = value;
        }
    }
}

