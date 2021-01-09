namespace Tanks.Lobby.ClientQuests.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x16122f289b6L)]
    public class BattleQuestProgressComponent : Component
    {
        public int CurrentValue { get; set; }
    }
}

