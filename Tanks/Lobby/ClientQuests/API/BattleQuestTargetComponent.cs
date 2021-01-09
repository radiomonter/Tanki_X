namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x16127b83ae9L)]
    public class BattleQuestTargetComponent : Component
    {
        public int TargetValue { get; set; }
    }
}

