namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class RemoveEffectGraphicsComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject effectPrefab;
        [SerializeField]
        private float effectLifeTime = 2f;
        [SerializeField]
        private Vector3 origin = Vector3.up;

        public GameObject EffectPrefab
        {
            get => 
                this.effectPrefab;
            set => 
                this.effectPrefab = value;
        }

        public float EffectLifeTime =>
            this.effectLifeTime;

        public Vector3 Origin =>
            this.origin;
    }
}

