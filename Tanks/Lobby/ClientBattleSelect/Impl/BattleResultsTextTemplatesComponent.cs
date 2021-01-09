namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;

    public class BattleResultsTextTemplatesComponent : LocalizedControl
    {
        public string TitleTextTemplate { get; set; }

        public string PlaceTextTemplate { get; set; }

        public string EarnedExperienceTextTemplate { get; set; }

        public string EarnedUpgradeTextTemplate { get; set; }

        public string RewardCountTextTemplate { get; set; }
    }
}

