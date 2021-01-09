namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class DoubleDamageSoundEffectComponent : BaseEffectSoundComponent<SoundController>
    {
        [SerializeField]
        private float startSoundDelaySec;
        [SerializeField]
        private float stopSoundDelaySec;
        [SerializeField]
        private float startSoundOffsetSec;
        [SerializeField]
        private float stopSoundOffsetSec;

        public override void BeginEffect()
        {
            base.StopSound.StopImmediately();
            base.StartSound.SetSoundActive();
        }

        public void RecalculatePlayingParameters()
        {
            SoundController startSound = base.StartSound;
            SoundController stopSound = base.StopSound;
            startSound.PlayingDelaySec = this.StartSoundDelaySec;
            stopSound.PlayingDelaySec = this.StopSoundDelaySec;
            startSound.PlayingOffsetSec = this.StartSoundOffsetSec;
            stopSound.PlayingOffsetSec = this.StopSoundOffsetSec;
        }

        public override void StopEffect()
        {
            base.StartSound.StopImmediately();
            base.StopSound.SetSoundActive();
        }

        public float StartSoundDelaySec
        {
            get => 
                this.startSoundDelaySec;
            set => 
                this.startSoundDelaySec = value;
        }

        public float StopSoundDelaySec
        {
            get => 
                this.stopSoundDelaySec;
            set => 
                this.stopSoundDelaySec = value;
        }

        public float StartSoundOffsetSec
        {
            get => 
                this.startSoundOffsetSec;
            set => 
                this.startSoundOffsetSec = value;
        }

        public float StopSoundOffsetSec
        {
            get => 
                this.stopSoundOffsetSec;
            set => 
                this.stopSoundOffsetSec = value;
        }
    }
}

