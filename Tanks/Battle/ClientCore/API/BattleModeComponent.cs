namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14d8f0c89e0L)]
    public class BattleModeComponent : Component
    {
        public Tanks.Battle.ClientCore.API.BattleMode BattleMode { get; set; }
    }
}

