namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class RicochetTargetHitSoundEffectComponent : BaseRicochetSoundEffectComponent
    {
        public override void Play(AudioSource sourceInstance)
        {
            sourceInstance.Play();
        }
    }
}

