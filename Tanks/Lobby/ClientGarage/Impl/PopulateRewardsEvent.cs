namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class PopulateRewardsEvent : Event
    {
        public PopulateRewardsEvent()
        {
            this.RewardsItems = new List<Entity>();
        }

        public List<Entity> RewardsItems { get; set; }
    }
}

