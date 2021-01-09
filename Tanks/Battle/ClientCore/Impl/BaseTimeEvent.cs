namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class BaseTimeEvent : Event
    {
        private int clientTime = ((int) (PreciseTime.Time * 1000.0));

        public int ClientTime
        {
            get => 
                this.clientTime;
            set => 
                this.clientTime = value;
        }
    }
}

