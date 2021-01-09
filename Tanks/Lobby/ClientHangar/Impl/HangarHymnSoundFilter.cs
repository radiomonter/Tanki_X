namespace Tanks.Lobby.ClientHangar.Impl
{
    using System;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class HangarHymnSoundFilter : SingleFadeSoundFilter
    {
        [SerializeField]
        private GameObject objectToDestroy;

        protected override bool CheckSoundIsPlaying() => 
            true;

        protected override void StopAndDestroy()
        {
            base.source.Stop();
            base.ResetFilter();
            DestroyObject(this.objectToDestroy);
        }

        protected override float FilterVolume
        {
            get => 
                HangarHymnSoundBehaviour.FILTER_VOLUME;
            set => 
                HangarHymnSoundBehaviour.FILTER_VOLUME = value;
        }
    }
}

