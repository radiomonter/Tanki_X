namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class UpdateUserRankEffectComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject effectPrefab;
        [SerializeField]
        private float finishEventTime = 7f;

        public GameObject EffectPrefab =>
            this.effectPrefab;

        public float FinishEventTime =>
            this.finishEventTime;
    }
}

