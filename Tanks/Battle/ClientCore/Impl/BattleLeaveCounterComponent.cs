namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15dbbb34f5cL)]
    public class BattleLeaveCounterComponent : Component
    {
        public long Value { get; set; }

        public int NeedGoodBattles { get; set; }
    }
}

