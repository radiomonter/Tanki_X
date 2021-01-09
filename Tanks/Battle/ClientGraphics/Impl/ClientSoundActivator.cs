namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class ClientSoundActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        [SerializeField]
        private int minProcessorCount = 2;
        [SerializeField]
        private int maxRealVoicesCountForWeakCPU = 0x20;
        [SerializeField]
        private int[] maxRealVoicesByQualityIndex;

        protected override void Activate()
        {
            this.UpdateAudioConfiguration();
        }

        private void OnAudioConfigurationChanged(bool deviceWasChanged)
        {
            if (deviceWasChanged)
            {
                this.UpdateAudioConfiguration();
            }
        }

        public void RegisterSystemsAndTemplates()
        {
            ECSBehaviour.EngineService.RegisterSystem(new ModuleEffectSoundSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HolyshieldSoundEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new DroneFlySoundEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new WeaponStreamHitSoundsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new EnergyInjectionSoundEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RageSoundEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankExplosionSoundSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HullSoundBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new WeaponSoundRotationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new DiscreteWeaponShotEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new StreamWeaponSoundEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MagazineSoundEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RailgunShotEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CTFSoundsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new VulcanSoundEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HitExplosionSoundSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MineCommonSoundsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new IceTrapSoundsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SpiderMineSoundsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BonusTakingSoundSystem());
            ECSBehaviour.EngineService.RegisterSystem(new GoldNotificationSoundSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftAimingSoundEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CaseSoundEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new RicochetSoundEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new IsisSoundEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftHitSoundEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ShaftShotSoundEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HammerHitSoundEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new AmbientMapSoundEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new AmbientZoneSoundEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MapNativeSoundsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SoundListenerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankFallingSoundEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankEngineSoundEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankFrictionSoundSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SoundListenerStateSystem());
            ECSBehaviour.EngineService.RegisterSystem(new KillTankSoundSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleSoundsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SoundListenerBattleSnapshotsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SoundListenerCleanerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HitFeedbackSoundSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HealthFeedbackSoundSystem());
            ECSBehaviour.EngineService.RegisterSystem(new WeaponEnergyFeedbackSoundSystem());
            ECSBehaviour.EngineService.RegisterSystem(new MapAnimatorTimerSystem());
            ECSBehaviour.EngineService.RegisterSystem(new LazySkyboxLoadingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new TankJumpSoundSystem());
        }

        private void UpdateAudioConfiguration()
        {
            AudioConfiguration config = AudioSettings.GetConfiguration();
            if (SystemInfo.processorCount <= this.minProcessorCount)
            {
                config.numRealVoices = this.maxRealVoicesCountForWeakCPU;
            }
            else
            {
                int currentQualityLevel = GraphicsSettings.INSTANCE.CurrentQualityLevel;
                config.numRealVoices = this.maxRealVoicesByQualityIndex[currentQualityLevel];
            }
            AudioSettings.Reset(config);
        }
    }
}

