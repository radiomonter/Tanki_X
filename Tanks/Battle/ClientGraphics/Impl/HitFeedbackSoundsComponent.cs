namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class HitFeedbackSoundsComponent : BehaviourComponent
    {
        [SerializeField]
        private WeaponFeedbackSoundBehaviour hammerHitFeedbackSoundAsset;
        [SerializeField]
        private WeaponFeedbackSoundBehaviour smokyHitFeedbackSoundAsset;
        [SerializeField]
        private WeaponFeedbackSoundBehaviour thunderHitFeedbackSoundAsset;
        [SerializeField]
        private WeaponFeedbackSoundBehaviour railgunHitFeedbackSoundAsset;
        [SerializeField]
        private WeaponFeedbackSoundBehaviour ricochetHitFeedbackSoundAsset;
        [SerializeField]
        private WeaponFeedbackSoundBehaviour shaftHitFeedbackSoundAsset;
        [SerializeField]
        private SoundController isisHealingFeedbackController;
        [SerializeField]
        private SoundController isisAttackFeedbackController;
        [SerializeField]
        private SoundController freezeWeaponAttackController;
        [SerializeField]
        private SoundController flamethrowerWeaponAttackController;

        public WeaponFeedbackSoundBehaviour SmokyHitFeedbackSoundAsset =>
            this.smokyHitFeedbackSoundAsset;

        public WeaponFeedbackSoundBehaviour ThunderHitFeedbackSoundAsset =>
            this.thunderHitFeedbackSoundAsset;

        public WeaponFeedbackSoundBehaviour RailgunHitFeedbackSoundAsset =>
            this.railgunHitFeedbackSoundAsset;

        public WeaponFeedbackSoundBehaviour RicochetHitFeedbackSoundAsset =>
            this.ricochetHitFeedbackSoundAsset;

        public WeaponFeedbackSoundBehaviour ShaftHitFeedbackSoundAsset =>
            this.shaftHitFeedbackSoundAsset;

        public WeaponFeedbackSoundBehaviour HammerHitFeedbackSoundAsset =>
            this.hammerHitFeedbackSoundAsset;

        public SoundController IsisHealingFeedbackController =>
            this.isisHealingFeedbackController;

        public SoundController IsisAttackFeedbackController =>
            this.isisAttackFeedbackController;

        public SoundController FreezeWeaponAttackController =>
            this.freezeWeaponAttackController;

        public SoundController FlamethrowerWeaponAttackController =>
            this.flamethrowerWeaponAttackController;
    }
}

