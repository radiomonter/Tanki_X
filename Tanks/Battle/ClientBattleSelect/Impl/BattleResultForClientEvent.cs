namespace Tanks.Battle.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15f94c4efc1L)]
    public class BattleResultForClientEvent : Event
    {
        public BattleResultForClient UserResultForClient { get; set; }
    }
}

