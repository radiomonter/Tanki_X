namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SquadConfigComponent : Component
    {
        public int MaxSquadSize { get; set; }
    }
}

