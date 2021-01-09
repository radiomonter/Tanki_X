namespace Tanks.Lobby.ClientHangar.Impl
{
    using System;
    using UnityEngine;

    public class HangarHymnSoundBehaviour : MonoBehaviour
    {
        public static float FILTER_VOLUME;
        [SerializeField]
        private HangarHymnSoundFilter introFilter;
        [SerializeField]
        private HangarHymnSoundFilter hangarLoopFilter;
        [SerializeField]
        private HangarHymnSoundFilter battleResultLoopFilter;

        private void Awake()
        {
            FILTER_VOLUME = 0f;
        }

        public void Play(bool playWithNitro)
        {
            if (playWithNitro)
            {
                this.introFilter.Play(-1f);
                this.hangarLoopFilter.Play(this.introFilter.Source.clip.length);
            }
            else
            {
                this.hangarLoopFilter.Play(-1f);
                this.battleResultLoopFilter.Play(-1f);
            }
        }

        public void Stop()
        {
            this.introFilter.Stop();
            this.hangarLoopFilter.Stop();
            this.battleResultLoopFilter.Stop();
        }
    }
}

