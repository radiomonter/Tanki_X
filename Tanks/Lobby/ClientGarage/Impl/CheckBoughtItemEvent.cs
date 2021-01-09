namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class CheckBoughtItemEvent : Event
    {
        private long itemId;
        private bool tutorialItemAlreadyBought;

        public CheckBoughtItemEvent(long itemId)
        {
            this.itemId = itemId;
        }

        public long ItemId
        {
            get => 
                this.itemId;
            set => 
                this.itemId = value;
        }

        public bool TutorialItemAlreadyBought
        {
            get => 
                this.tutorialItemAlreadyBought;
            set => 
                this.tutorialItemAlreadyBought = value;
        }
    }
}

