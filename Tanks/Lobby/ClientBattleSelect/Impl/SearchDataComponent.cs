namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientBattleSelect.API;

    public class SearchDataComponent : Component
    {
        public SearchDataComponent(Tanks.Lobby.ClientBattleSelect.API.BattleEntry battleEntry, int indexInSearchResult)
        {
            this.BattleEntry = battleEntry;
            this.IndexInSearchResult = indexInSearchResult;
        }

        public Tanks.Lobby.ClientBattleSelect.API.BattleEntry BattleEntry { get; set; }

        public int IndexInSearchResult { get; set; }
    }
}

