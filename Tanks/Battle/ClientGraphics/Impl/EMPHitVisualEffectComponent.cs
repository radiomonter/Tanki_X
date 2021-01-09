namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class EMPHitVisualEffectComponent : BehaviourComponent
    {
        [SerializeField]
        private ParticleSystem HitPrefab;

        public ParticleSystem EmpHitPrefab =>
            this.HitPrefab;
    }
}

