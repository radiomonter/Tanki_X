namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class EnterBattleEvent : Event
    {
        public Tanks.Battle.ClientCore.API.TeamColor TeamColor { get; set; }
    }
}

