namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class RPMSoundBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float rpm;
        [SerializeField]
        private ActiveRPMSoundModifier activeRPMSound;
        [SerializeField]
        private NormalRPMSoundModifier normalRPMSound;
        [SerializeField]
        private float rangeBeginRPM;
        [SerializeField]
        private float rangeEndRPM;
        [SerializeField]
        private HullSoundEngineController hullSoundEngine;

        public void Build(HullSoundEngineController engine, float prevRPM, float nextRPM, float blendRange)
        {
            this.hullSoundEngine = engine;
            this.rangeBeginRPM = Mathf.Lerp(this.rpm, prevRPM, blendRange);
            this.rangeEndRPM = Mathf.Lerp(this.rpm, nextRPM, blendRange);
            this.activeRPMSound.Build(this);
            this.normalRPMSound.Build(this);
        }

        public void Play(float volume)
        {
            this.activeRPMSound.Play(volume);
            this.normalRPMSound.Play(volume);
        }

        public void Stop()
        {
            this.activeRPMSound.Stop();
            this.normalRPMSound.Stop();
        }

        public HullSoundEngineController HullSoundEngine =>
            this.hullSoundEngine;

        public float RPM =>
            this.rpm;

        public float RangeBeginRpm =>
            this.rangeBeginRPM;

        public float RangeEndRpm =>
            this.rangeEndRPM;

        public bool NeedToStop =>
            this.activeRPMSound.NeedToStop && this.normalRPMSound.NeedToStop;
    }
}

