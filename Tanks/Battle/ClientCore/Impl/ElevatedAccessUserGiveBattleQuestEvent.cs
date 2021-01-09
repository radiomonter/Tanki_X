namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1617562ace8L)]
    public class ElevatedAccessUserGiveBattleQuestEvent : Event
    {
        public string QuestPath { get; set; }
    }
}

