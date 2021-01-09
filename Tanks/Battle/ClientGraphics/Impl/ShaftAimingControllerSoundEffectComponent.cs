namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;

    [SerialVersionUID(0x8d2e6e11b0330aaL)]
    public class ShaftAimingControllerSoundEffectComponent : AbstractShaftSoundEffectComponent<SoundController>
    {
        public override void Play()
        {
            base.soundComponent.FadeIn();
        }

        public override void Stop()
        {
            base.soundComponent.FadeOut();
        }
    }
}

