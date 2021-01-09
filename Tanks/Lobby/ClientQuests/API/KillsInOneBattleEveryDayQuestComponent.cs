namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x157d2afea69L)]
    public class KillsInOneBattleEveryDayQuestComponent : Component
    {
        public int Days { get; set; }
    }
}

