namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public abstract class AbstractRPMSoundModifier : MonoBehaviour
    {
        private const float SELF_LOAD_MODIFIER_COEFF_B = 0f;
        private const float REMOTE_LOAD_MODIFIER_COEFF_B = 0.98f;
        private const float SELF_LOAD_MODIFIER_COEFF_K = 1f;
        private const float REMOTE_LOAD_MODIFIER_COEFF_K = 2f;
        private const int SELF_SOUND_PRIORITY = 0x80;
        private const int REMOTE_SOUND_PRIORITY = 0x80;
        [SerializeField]
        private AudioSource source;
        [SerializeField]
        private RPMSoundBehaviour rpmSoundBehaviour;
        [SerializeField]
        private AbstractRPMSoundUpdater childUpdater;
        [SerializeField]
        private float targetRPM;
        [SerializeField]
        private bool needToStop;
        [SerializeField]
        private float rpmSoundVolume;
        private float loadModifierB;
        private float loadModifierK;

        protected AbstractRPMSoundModifier()
        {
        }

        protected virtual void Awake()
        {
            this.source.Stop();
            this.needToStop = false;
            this.rpmSoundVolume = 1f;
            if (this.rpmSoundBehaviour.HullSoundEngine.SelfEngine)
            {
                this.loadModifierK = 1f;
                this.loadModifierB = 0f;
                this.source.priority = 0x80;
            }
            else
            {
                this.loadModifierK = 2f;
                this.loadModifierB = 0.98f;
                this.source.priority = 0x80;
            }
        }

        public void Build(RPMSoundBehaviour rpmSoundBehaviour)
        {
            this.rpmSoundBehaviour = rpmSoundBehaviour;
            this.targetRPM = rpmSoundBehaviour.RPM;
            HullSoundEngineController hullSoundEngine = rpmSoundBehaviour.HullSoundEngine;
            if (hullSoundEngine.UseAudioFilters)
            {
                this.InitChildUpdater<RPMAudioFilter, RPMVolumeUpdater>();
            }
            else
            {
                this.InitChildUpdater<RPMVolumeUpdater, RPMAudioFilter>();
            }
            this.childUpdater.Build(hullSoundEngine, this, rpmSoundBehaviour);
        }

        protected float CalculateLinearLoadModifier(float smoothedLoad) => 
            Mathf.Sqrt(Mathf.Clamp01(Mathf.SmoothStep(-this.loadModifierB, this.loadModifierK - this.loadModifierB, smoothedLoad)));

        public abstract float CalculateLoadPartForModifier(float smoothedLoad);
        public float CalculateModifier(float smoothedRPM, float smoothedLoad)
        {
            float f = this.CalculateRPMRangeFactor(smoothedRPM);
            float num2 = this.CalculateLoadPartForModifier(smoothedLoad);
            return (((smoothedRPM < this.targetRPM) ? Mathf.Sqrt(f) : Mathf.Sqrt(1f - f)) * num2);
        }

        private float CalculateRPMRangeFactor(float currentRPM)
        {
            float rangeBeginRpm = this.rpmSoundBehaviour.RangeBeginRpm;
            float rangeEndRpm = this.rpmSoundBehaviour.RangeEndRpm;
            return ((currentRPM - rangeBeginRpm) / (rangeEndRpm - rangeBeginRpm));
        }

        public abstract bool CheckLoad(float smoothedLoad);
        private void InitChildUpdater<TAdd, TRemove>() where TAdd: AbstractRPMSoundUpdater where TRemove: AbstractRPMSoundUpdater
        {
            TRemove component = base.gameObject.GetComponent<TRemove>();
            if (component != null)
            {
                DestroyImmediate(component);
            }
            this.childUpdater = base.gameObject.GetComponent<TAdd>();
            if (this.childUpdater == null)
            {
                this.childUpdater = base.gameObject.AddComponent<TAdd>();
            }
        }

        public void Play(float volume)
        {
            this.rpmSoundVolume = volume;
            this.needToStop = false;
            this.childUpdater.Play();
        }

        public void Stop()
        {
            this.needToStop = false;
            this.childUpdater.Stop();
        }

        public AudioSource Source =>
            this.source;

        public float RpmSoundVolume =>
            Mathf.Min(this.rpmSoundVolume, 1f);

        public bool NeedToStop
        {
            get => 
                this.needToStop;
            set => 
                this.needToStop = value;
        }

        public RPMSoundBehaviour RpmSoundBehaviour =>
            this.rpmSoundBehaviour;
    }
}

