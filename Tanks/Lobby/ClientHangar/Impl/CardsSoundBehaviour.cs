namespace Tanks.Lobby.ClientHangar.Impl
{
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class CardsSoundBehaviour : MonoBehaviour
    {
        [SerializeField]
        private SoundController openCardsContainerSource;

        public SoundController OpenCardsContainerSource =>
            this.openCardsContainerSource;
    }
}

