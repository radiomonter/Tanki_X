namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1612cb0e3a9L)]
    public class BattleQuestRewardComponent : Component
    {
        public Tanks.Lobby.ClientQuests.API.BattleQuestReward BattleQuestReward { get; set; }

        public int Quantity { get; set; }
    }
}

