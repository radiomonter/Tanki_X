namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class QuestVariationsComponent : Component
    {
        public List<QuestParameters> Quests { get; set; }
    }
}

