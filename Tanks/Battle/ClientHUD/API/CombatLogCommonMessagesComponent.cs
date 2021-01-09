﻿namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CombatLogCommonMessagesComponent : Component
    {
        public string KillMessage { get; set; }

        public string UserJoinBattleMessage { get; set; }

        public string UserLeaveBattleMessage { get; set; }

        public string UserChangedTankMessage { get; set; }

        public string SuicideMessage { get; set; }

        public string GoldScheduledMessage { get; set; }

        public string UserGoldScheduledMessage { get; set; }

        public string GoldTakenMessage { get; set; }

        public string KillStreakMessage { get; set; }
    }
}

