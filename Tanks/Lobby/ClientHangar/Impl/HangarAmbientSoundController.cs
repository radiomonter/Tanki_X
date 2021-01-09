namespace Tanks.Lobby.ClientHangar.Impl
{
    using System;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class HangarAmbientSoundController : MonoBehaviour
    {
        [SerializeField]
        private AmbientSoundFilter background;
        [SerializeField]
        private HangarHymnSoundBehaviour hymn;

        public void Play(bool playWithNitro)
        {
            this.background.Play(-1f);
            this.hymn.Play(playWithNitro);
        }

        public void Stop()
        {
            this.background.Stop();
            this.hymn.Stop();
        }

        private void Update()
        {
            if ((this.background == null) && (this.hymn == null))
            {
                DestroyObject(base.gameObject);
            }
        }
    }
}

