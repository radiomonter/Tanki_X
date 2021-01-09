namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class RageSoundEffectComponent : BehaviourComponent
    {
        [SerializeField]
        private RageSoundEffectBehaviour asset;

        public RageSoundEffectBehaviour Asset =>
            this.asset;
    }
}

