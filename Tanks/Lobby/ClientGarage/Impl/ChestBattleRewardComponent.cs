namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4ea02c4bd598eL)]
    public class ChestBattleRewardComponent : Component
    {
        public long ChestId { get; set; }
    }
}

