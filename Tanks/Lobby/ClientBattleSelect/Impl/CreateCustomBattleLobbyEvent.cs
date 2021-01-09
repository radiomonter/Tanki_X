namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15c7d41adf6L)]
    public class CreateCustomBattleLobbyEvent : Event
    {
        public ClientBattleParams Params { get; set; }
    }
}

