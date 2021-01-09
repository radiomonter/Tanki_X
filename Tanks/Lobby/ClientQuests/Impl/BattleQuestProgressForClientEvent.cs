namespace Tanks.Lobby.ClientQuests.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x16122e4c9b9L)]
    public class BattleQuestProgressForClientEvent : Event
    {
        public int ProgressDelta { get; set; }
    }
}

