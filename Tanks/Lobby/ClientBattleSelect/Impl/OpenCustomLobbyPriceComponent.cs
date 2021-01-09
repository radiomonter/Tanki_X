namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x168945c55bdL)]
    public class OpenCustomLobbyPriceComponent : Component
    {
        public long OpenPrice { get; set; }
    }
}

