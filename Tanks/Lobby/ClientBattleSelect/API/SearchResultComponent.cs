namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SearchResultComponent : Component
    {
        private List<BattleEntry> pinnedBattles = new List<BattleEntry>();
        private List<PersonalBattleInfo> personalInfos = new List<PersonalBattleInfo>();

        public List<BattleEntry> PinnedBattles
        {
            get => 
                this.pinnedBattles;
            set => 
                this.pinnedBattles = value;
        }

        public List<PersonalBattleInfo> PersonalInfos
        {
            get => 
                this.personalInfos;
            set => 
                this.personalInfos = value;
        }

        public int BattlesCount { get; set; }
    }
}

