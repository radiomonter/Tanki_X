namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using UnityEngine;

    public abstract class SingleFadeSoundFilter : FadeSoundFilter
    {
        protected const float TIMEOUT_SEC = 0.3f;

        protected SingleFadeSoundFilter()
        {
        }

        protected override bool CheckSoundIsPlaying() => 
            base.source.isPlaying;

        protected override void StopAndDestroy()
        {
            base.source.Stop();
            DestroyObject(base.gameObject, 0.3f);
            base.ResetFilter();
        }
    }
}

