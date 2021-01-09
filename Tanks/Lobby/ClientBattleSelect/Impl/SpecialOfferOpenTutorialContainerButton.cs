namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;

    public class SpecialOfferOpenTutorialContainerButton : BehaviourComponent
    {
        public long containerId;
        public int quantity;
        public Action onOpen;
    }
}

