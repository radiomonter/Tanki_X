namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class BattleSelectScreenHeaderTextComponent : Component
    {
        public string HeaderText { get; set; }

        public float HeaderTextShowDelaySeconds { get; set; }
    }
}

