namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class UpdateUserRankSoundEffectAssetComponent : BehaviourComponent
    {
        [SerializeField]
        private AudioSource selfUserRankSource;
        [SerializeField]
        private AudioSource remoteUserRankSource;

        public AudioSource SelfUserRankSource =>
            this.selfUserRankSource;

        public AudioSource RemoteUserRankSource =>
            this.remoteUserRankSource;
    }
}

