namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;

    public class ShaftRollCooldownSoundEffectComponent : AbstractShaftSoundEffectComponent<SoundController>
    {
        public override void Play()
        {
            base.soundComponent.SetSoundActive();
            base.soundComponent.FadeOut();
        }

        public void SetFadeOutTime(float fadeOutTimeSec)
        {
            base.soundComponent.FadeOutTimeSec = fadeOutTimeSec;
        }

        public override void Stop()
        {
            base.soundComponent.StopImmediately();
        }
    }
}

