namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4e91d2c2dba4dL)]
    public class GameplayChestScoreComponent : Component
    {
        public long Current { get; set; }

        public long Limit { get; set; }
    }
}

