namespace Tanks.Lobby.ClientBattleSelect.API
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class GameModesDescriptionData
    {
        public const string configPath = "localization/battle_mode";

        public Dictionary<BattleMode, string> battleModeLocalization { get; set; }
    }
}

