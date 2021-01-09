namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class SoundListenerComponent : BehaviourComponent
    {
        [SerializeField]
        private float delayForLobbyState = 0.33f;
        [SerializeField]
        private float delayForBattleEnterState = 0.05f;
        [SerializeField]
        private float delayForBattleState = 1.5f;

        private void Awake()
        {
            AudioListener.pause = false;
        }

        private void OnApplicationQuit()
        {
            AudioListener.pause = true;
        }

        private void OnDestroy()
        {
            AudioListener.pause = true;
        }

        public float DelayForLobbyState =>
            this.delayForLobbyState;

        public float DelayForBattleEnterState =>
            this.delayForBattleEnterState;

        public float DelayForBattleState =>
            this.delayForBattleState;
    }
}

