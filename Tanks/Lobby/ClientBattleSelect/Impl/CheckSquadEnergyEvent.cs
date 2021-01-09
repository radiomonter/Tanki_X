namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CheckSquadEnergyEvent : Event
    {
        public bool HaveEnoughtEnergyForEntrance { get; set; }
    }
}

