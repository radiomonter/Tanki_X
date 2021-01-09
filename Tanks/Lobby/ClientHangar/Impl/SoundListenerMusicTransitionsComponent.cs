namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class SoundListenerMusicTransitionsComponent : BehaviourComponent
    {
        [SerializeField]
        private float musicTransitionSec = 0.5f;
        [SerializeField]
        private float transitionCardsContainerTheme = 0.2f;
        [SerializeField]
        private float transitionModuleTheme = 0.2f;

        public float MusicTransitionSec =>
            this.musicTransitionSec;

        public float TransitionCardsContainerTheme =>
            this.transitionCardsContainerTheme;

        public float TransitionModuleTheme =>
            this.transitionModuleTheme;
    }
}

