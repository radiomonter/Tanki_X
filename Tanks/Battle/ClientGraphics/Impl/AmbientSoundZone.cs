namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class AmbientSoundZone : AmbientSoundFilter
    {
        [SerializeField]
        private AmbientZoneSoundEffect ambientZoneSoundEffect;

        public void FadeIn()
        {
            this.ambientZoneSoundEffect.RegisterPlayingAmbientZone(this);
            base.Play(-1f);
        }

        public void FadeOut()
        {
            base.Stop();
        }

        protected override void StopAndDestroy()
        {
            base.source.Stop();
            base.ResetFilter();
            this.ambientZoneSoundEffect.UnregisterPlayingAmbientZone(this);
            this.ambientZoneSoundEffect.FinalizeEffect();
        }
    }
}

