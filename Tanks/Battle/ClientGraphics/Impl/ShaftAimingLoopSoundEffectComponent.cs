namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;

    public class ShaftAimingLoopSoundEffectComponent : AbstractShaftSoundEffectComponent<SoundController>
    {
        public override void Play()
        {
            base.soundComponent.SetSoundActive();
        }

        public void SetDelay(float delayTimeSec)
        {
            base.soundComponent.PlayingDelaySec = delayTimeSec;
        }

        public override void Stop()
        {
            base.soundComponent.FadeOut();
        }
    }
}

