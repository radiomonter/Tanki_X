namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class TutorialRewardDataComponent : Component
    {
        public long CrysCount { get; set; }

        public List<Reward> Rewards { get; set; }
    }
}

