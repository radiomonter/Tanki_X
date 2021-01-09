namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientBattleSelect.API;

    [Shared, SerialVersionUID(0x1547705ec25L)]
    public class PersonalBattleInfoEvent : Event
    {
        public PersonalBattleInfo Info { get; set; }
    }
}

