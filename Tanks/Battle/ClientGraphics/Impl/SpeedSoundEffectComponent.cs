﻿namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;

    public class SpeedSoundEffectComponent : BaseEffectSoundComponent<AudioSource>
    {
        public override void BeginEffect()
        {
            base.StopSound.Stop();
            base.StartSound.Play();
        }

        public override void StopEffect()
        {
            base.StartSound.Stop();
            base.StopSound.Play();
        }
    }
}

