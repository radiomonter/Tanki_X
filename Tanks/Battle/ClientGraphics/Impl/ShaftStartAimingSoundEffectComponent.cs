namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;

    public class ShaftStartAimingSoundEffectComponent : AbstractShaftSoundEffectComponent<SoundController>
    {
        public override void Play()
        {
            base.soundComponent.SetSoundActive();
        }

        public override void Stop()
        {
            base.soundComponent.FadeOut();
        }

        public float StartAimingDurationSec =>
            base.soundComponent.Source.clip.length;
    }
}

