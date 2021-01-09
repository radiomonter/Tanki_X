namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class RFX4_AudioCurves : MonoBehaviour
    {
        public AnimationCurve AudioCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public float GraphTimeMultiplier = 1f;
        public bool IsLoop;
        private bool canUpdate;
        private float startTime;
        private AudioSource audioSource;
        private float startVolume;

        private void Awake()
        {
            this.audioSource = base.GetComponent<AudioSource>();
            this.startVolume = this.audioSource.volume;
            this.audioSource.volume = this.AudioCurve.Evaluate(0f);
        }

        private void OnEnable()
        {
            this.startTime = Time.time;
            this.canUpdate = true;
        }

        private void Update()
        {
            float num = Time.time - this.startTime;
            if (this.canUpdate)
            {
                float num2 = this.AudioCurve.Evaluate(num / this.GraphTimeMultiplier) * this.startVolume;
                this.audioSource.volume = num2;
            }
            if (num >= this.GraphTimeMultiplier)
            {
                if (this.IsLoop)
                {
                    this.startTime = Time.time;
                }
                else
                {
                    this.canUpdate = false;
                }
            }
        }
    }
}

