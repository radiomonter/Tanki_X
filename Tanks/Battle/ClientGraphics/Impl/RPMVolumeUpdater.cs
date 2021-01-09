namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class RPMVolumeUpdater : AbstractRPMSoundUpdater
    {
        [SerializeField]
        private RPMVolumeUpdaterFinishBehaviour rpmVolumeUpdaterFinishBehaviour;

        public override void Build(HullSoundEngineController engine, AbstractRPMSoundModifier abstractRPMSoundModifier, RPMSoundBehaviour rpmSoundBehaviour)
        {
            base.Build(engine, abstractRPMSoundModifier, rpmSoundBehaviour);
            this.rpmVolumeUpdaterFinishBehaviour = base.gameObject.AddComponent<RPMVolumeUpdaterFinishBehaviour>();
            this.rpmVolumeUpdaterFinishBehaviour.Build(base.parentModifier.Source);
        }

        protected override void OnDisable()
        {
            if (base.alive)
            {
                this.rpmVolumeUpdaterFinishBehaviour.enabled = true;
            }
        }

        protected override void OnEnable()
        {
            this.UpdateVolume();
            this.rpmVolumeUpdaterFinishBehaviour.enabled = false;
            AudioSource source = base.parentModifier.Source;
            if (!source.isPlaying)
            {
                source.Play();
            }
        }

        private void Update()
        {
            this.UpdateVolume();
        }

        private void UpdateVolume()
        {
            AudioSource source = base.parentModifier.Source;
            float engineRpm = base.engine.EngineRpm;
            float engineLoad = base.engine.EngineLoad;
            float rpmSoundVolume = base.parentModifier.RpmSoundVolume;
            if (!base.engine.IsRPMWithinRange(base.rpmSoundBehaviour, base.engine.EngineRpm))
            {
                source.volume = 0f;
                base.parentModifier.NeedToStop = true;
            }
            else if (!base.parentModifier.CheckLoad(base.engine.EngineLoad))
            {
                source.volume = 0f;
            }
            else
            {
                source.volume = rpmSoundVolume * base.parentModifier.CalculateModifier(engineRpm, engineLoad);
            }
        }
    }
}

