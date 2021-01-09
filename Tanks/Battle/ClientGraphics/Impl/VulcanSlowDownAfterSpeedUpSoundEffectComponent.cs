namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class VulcanSlowDownAfterSpeedUpSoundEffectComponent : AbstractVulcanSoundEffectComponent
    {
        [SerializeField]
        private float additionalStartTimeOffset;

        public float AdditionalStartTimeOffset
        {
            get => 
                this.additionalStartTimeOffset;
            set => 
                this.additionalStartTimeOffset = value;
        }
    }
}

