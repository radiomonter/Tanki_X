namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15ba9742cc2L)]
    public class QuestRewardComponent : Component
    {
        public Dictionary<long, int> Reward { get; set; }
    }
}

