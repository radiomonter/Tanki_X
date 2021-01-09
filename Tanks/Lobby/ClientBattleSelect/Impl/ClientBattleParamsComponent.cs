namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15ce9ae5bfbL)]
    public class ClientBattleParamsComponent : Component
    {
        public ClientBattleParams Params { get; set; }
    }
}

