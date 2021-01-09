namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class PersonalBattleInfoComponent : Component
    {
        public PersonalBattleInfo Info { get; set; }
    }
}

