namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15cb0cec154L)]
    public class UpdateBattleParamsEvent : Event
    {
        public ClientBattleParams Params { get; set; }
    }
}

