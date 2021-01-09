namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class SoundListenerBattleMixerSnapshotTransitionComponent : BehaviourComponent
    {
        [SerializeField]
        private float transitionTimeToSilentAfterRoundFinish = 0.25f;
        [SerializeField]
        private float transitionTimeToSilentAfterExitBattle = 0.5f;
        [SerializeField]
        private float transitionTimeToMelodySilent = 0.5f;
        [SerializeField]
        private float transitionToLoudTimeInBattle = 0.5f;
        [SerializeField]
        private float transitionToLoudTimeInSelfUserMode = 0.5f;

        public float TransitionTimeToSilentAfterRoundFinish =>
            this.transitionTimeToSilentAfterRoundFinish;

        public float TransitionTimeToSilentAfterExitBattle =>
            this.transitionTimeToSilentAfterExitBattle;

        public float TransitionToLoudTimeInBattle =>
            this.transitionToLoudTimeInBattle;

        public float TransitionToLoudTimeInSelfUserMode =>
            this.transitionToLoudTimeInSelfUserMode;

        public float TransitionTimeToMelodySilent =>
            this.transitionTimeToMelodySilent;
    }
}

