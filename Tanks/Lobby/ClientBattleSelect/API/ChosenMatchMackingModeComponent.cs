namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ChosenMatchMackingModeComponent : Component
    {
        public Entity ModeEntity { get; set; }
    }
}

