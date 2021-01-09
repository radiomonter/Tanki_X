namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class HangarMatchLobbySoundComponent : BehaviourComponent
    {
        [SerializeField]
        private AudioSource[] sources;
        private AudioSource lastSource;

        public void Play()
        {
            if (this.lastSource != null)
            {
                this.lastSource.Stop();
            }
            int index = Random.Range(0, this.sources.Length);
            this.lastSource = this.sources[index];
            this.lastSource.Play();
        }
    }
}

