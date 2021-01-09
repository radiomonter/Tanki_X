namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class GetEnergyCountTutorialValidationDataEvent : Event
    {
        private long _quantums;

        public long Quantums
        {
            get => 
                this._quantums;
            set => 
                this._quantums = value;
        }
    }
}

