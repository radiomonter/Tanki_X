namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14e6d523d1aL)]
    public class SearchResultEvent : Event
    {
        public List<BattleEntry> NewBattleEntries { get; set; }

        public List<PersonalBattleInfo> NewPersonalBattleInfos { get; set; }

        public int BattlesCount { get; set; }
    }
}

