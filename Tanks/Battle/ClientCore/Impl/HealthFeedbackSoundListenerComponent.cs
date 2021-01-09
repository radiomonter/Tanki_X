namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class HealthFeedbackSoundListenerComponent : BehaviourComponent
    {
        [SerializeField]
        private AudioReverbFilter filter;
        [SerializeField]
        private HealthFeedbackListenerPreset normalHealthPreset;
        [SerializeField]
        private HealthFeedbackListenerPreset lowHealthPreset;
        [SerializeField]
        private float maxHealthPercentForSound = 0.3f;
        [SerializeField]
        private float enterTimeSec = 0.5f;
        [SerializeField]
        private float exitTimeSec = 0.5f;
        [SerializeField]
        private AnimationCurve toLowHealthStateCurve;
        [SerializeField]
        private AnimationCurve toNormalHealthStateCurve;
        private float presetInterpolator;
        private float speed;
        private AnimationCurve curve;

        private void ApplyPresetInterpolator()
        {
            this.presetInterpolator = Mathf.Clamp01(this.presetInterpolator);
            float t = this.curve.Evaluate(this.presetInterpolator);
            this.filter.dryLevel = Mathf.Lerp(this.normalHealthPreset.DryLevel, this.lowHealthPreset.DryLevel, t);
            this.filter.room = Mathf.Lerp(this.normalHealthPreset.Room, this.lowHealthPreset.Room, t);
            this.filter.roomHF = Mathf.Lerp(this.normalHealthPreset.RoomHf, this.lowHealthPreset.RoomHf, t);
            this.filter.roomLF = Mathf.Lerp(this.normalHealthPreset.RoomLf, this.lowHealthPreset.RoomLf, t);
            this.filter.decayTime = Mathf.Lerp(this.normalHealthPreset.DecayTime, this.lowHealthPreset.DecayTime, t);
            this.filter.decayHFRatio = Mathf.Lerp(this.normalHealthPreset.DecayHfRatio, this.lowHealthPreset.DecayHfRatio, t);
            this.filter.reflectionsLevel = Mathf.Lerp(this.normalHealthPreset.ReflectionsLevel, this.lowHealthPreset.ReflectionsLevel, t);
            this.filter.reflectionsDelay = Mathf.Lerp(this.normalHealthPreset.ReflectionsDelay, this.lowHealthPreset.ReflectionsDelay, t);
            this.filter.reverbLevel = Mathf.Lerp(this.normalHealthPreset.ReverbLevel, this.lowHealthPreset.ReverbLevel, t);
            this.filter.reverbDelay = Mathf.Lerp(this.normalHealthPreset.ReverbDelay, this.lowHealthPreset.ReverbDelay, t);
            this.filter.hfReference = Mathf.Lerp(this.normalHealthPreset.HfReference, this.lowHealthPreset.HfReference, t);
            this.filter.lfReference = Mathf.Lerp(this.normalHealthPreset.LfReference, this.lowHealthPreset.LfReference, t);
            this.filter.diffusion = Mathf.Lerp(this.normalHealthPreset.Diffusion, this.lowHealthPreset.Diffusion, t);
            this.filter.density = Mathf.Lerp(this.normalHealthPreset.Density, this.lowHealthPreset.Density, t);
        }

        private void OnDisable()
        {
            this.presetInterpolator = 0f;
            if (this.filter)
            {
                this.filter.enabled = false;
            }
        }

        private void OnEnable()
        {
            this.filter.enabled = true;
        }

        public void ResetHealthFeedbackData()
        {
            base.enabled = false;
            this.speed = 0f;
            this.presetInterpolator = 0f;
        }

        private void StartRunning(float speed, AnimationCurve curve)
        {
            this.speed = speed;
            this.curve = curve;
            this.ApplyPresetInterpolator();
            base.enabled = true;
        }

        public void SwitchToLowHealthMode()
        {
            this.StartRunning(1f / this.enterTimeSec, this.toLowHealthStateCurve);
        }

        public void SwitchToNormalHealthMode()
        {
            this.StartRunning(-1f / this.exitTimeSec, this.toNormalHealthStateCurve);
        }

        private void Update()
        {
            bool flag = this.presetInterpolator >= 1f;
            this.presetInterpolator += this.speed * Time.deltaTime;
            bool flag2 = this.presetInterpolator >= 1f;
            if (this.presetInterpolator <= 0f)
            {
                base.enabled = false;
            }
            else if (!flag || !flag2)
            {
                this.ApplyPresetInterpolator();
            }
        }

        public float MaxHealthPercentForSound =>
            this.maxHealthPercentForSound;

        [Serializable]
        private class HealthFeedbackListenerPreset
        {
            [SerializeField]
            private float dryLevel;
            [SerializeField]
            private float room;
            [SerializeField]
            private float roomHF;
            [SerializeField]
            private float roomLF;
            [SerializeField]
            private float decayTime;
            [SerializeField]
            private float decayHFRatio;
            [SerializeField]
            private float reflectionsLevel;
            [SerializeField]
            private float reflectionsDelay;
            [SerializeField]
            private float reverbLevel;
            [SerializeField]
            private float reverbDelay;
            [SerializeField]
            private float hfReference;
            [SerializeField]
            private float lfReference;
            [SerializeField]
            private float diffusion;
            [SerializeField]
            private float density;

            public float DryLevel =>
                this.dryLevel;

            public float Room =>
                this.room;

            public float RoomHf =>
                this.roomHF;

            public float DecayTime =>
                this.decayTime;

            public float DecayHfRatio =>
                this.decayHFRatio;

            public float RoomLf =>
                this.roomLF;

            public float ReflectionsLevel =>
                this.reflectionsLevel;

            public float ReflectionsDelay =>
                this.reflectionsDelay;

            public float ReverbLevel =>
                this.reverbLevel;

            public float ReverbDelay =>
                this.reverbDelay;

            public float HfReference =>
                this.hfReference;

            public float LfReference =>
                this.lfReference;

            public float Diffusion =>
                this.diffusion;

            public float Density =>
                this.density;
        }
    }
}

