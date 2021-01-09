namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.Audio;

    public class ActiveRPMSoundModifier : AbstractRPMSoundModifier
    {
        [SerializeField]
        private AudioMixerGroup selfActiveGroup;
        [SerializeField]
        private AudioMixerGroup remoteActiveGroup;

        protected override void Awake()
        {
            base.Awake();
            base.Source.outputAudioMixerGroup = !base.RpmSoundBehaviour.HullSoundEngine.SelfEngine ? this.remoteActiveGroup : this.selfActiveGroup;
        }

        public override float CalculateLoadPartForModifier(float smoothedLoad) => 
            Mathf.Sqrt(base.CalculateLinearLoadModifier(smoothedLoad));

        public override bool CheckLoad(float smoothedLoad) => 
            smoothedLoad > 0f;
    }
}

