namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    [Shared, SerialVersionUID(0x15b1392d2f8L)]
    public class ReservedInBattleInfoComponent : Component
    {
        public Date ExitTime { get; set; }

        public long Map { get; set; }

        public Tanks.Battle.ClientCore.API.BattleMode BattleMode { get; set; }
    }
}

