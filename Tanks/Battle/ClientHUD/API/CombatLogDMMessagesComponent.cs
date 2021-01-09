namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CombatLogDMMessagesComponent : Component
    {
        public string BattleStartMessage { get; set; }
    }
}

