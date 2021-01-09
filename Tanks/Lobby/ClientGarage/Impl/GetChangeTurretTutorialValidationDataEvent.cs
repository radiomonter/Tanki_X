namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.InteropServices;

    public class GetChangeTurretTutorialValidationDataEvent : Event
    {
        private long stepId;
        private long itemId;
        private long battlesCount;
        private bool tutorialItemAlreadyMounted;
        private bool tutorialItemAlreadyBought;
        private long mountedWeaponId;

        public GetChangeTurretTutorialValidationDataEvent(long stepId, long itemId = 0L)
        {
            this.stepId = stepId;
            this.itemId = itemId;
        }

        public long ItemId
        {
            get => 
                this.itemId;
            set => 
                this.itemId = value;
        }

        public long StepId
        {
            get => 
                this.stepId;
            set => 
                this.stepId = value;
        }

        public long BattlesCount
        {
            get => 
                this.battlesCount;
            set => 
                this.battlesCount = value;
        }

        public bool TutorialItemAlreadyMounted
        {
            get => 
                this.tutorialItemAlreadyMounted;
            set => 
                this.tutorialItemAlreadyMounted = value;
        }

        public bool TutorialItemAlreadyBought
        {
            get => 
                this.tutorialItemAlreadyBought;
            set => 
                this.tutorialItemAlreadyBought = value;
        }

        public long MountedWeaponId
        {
            get => 
                this.mountedWeaponId;
            set => 
                this.mountedWeaponId = value;
        }
    }
}

