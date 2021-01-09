namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class HolyshieldSoundEffectAssetComponent : BehaviourComponent
    {
        [SerializeField]
        private SoundController asset;

        public SoundController Asset =>
            this.asset;
    }
}

