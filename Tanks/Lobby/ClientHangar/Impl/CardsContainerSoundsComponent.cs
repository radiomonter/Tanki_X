namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using UnityEngine;

    public class CardsContainerSoundsComponent : BehaviourComponent
    {
        [SerializeField]
        private CardsSoundBehaviour cardsSounds;

        public CardsSoundBehaviour CardsSounds =>
            this.cardsSounds;
    }
}

