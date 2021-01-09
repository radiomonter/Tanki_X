namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class KillTankSoundEffectComponent : BehaviourComponent
    {
        [SerializeField]
        private KillTankSoundEffectBehaviour effectPrefab;

        public KillTankSoundEffectBehaviour EffectPrefab =>
            this.effectPrefab;
    }
}

