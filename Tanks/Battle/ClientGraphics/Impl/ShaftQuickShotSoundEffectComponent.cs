﻿namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;

    public class ShaftQuickShotSoundEffectComponent : AbstractShaftSoundEffectComponent<AudioSource>
    {
        public override void Play()
        {
            base.soundComponent.Play();
        }

        public override void Stop()
        {
            base.soundComponent.Stop();
        }
    }
}

