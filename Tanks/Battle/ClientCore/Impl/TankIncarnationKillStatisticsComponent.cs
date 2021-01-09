namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15b47440d8fL)]
    public class TankIncarnationKillStatisticsComponent : Component
    {
        public int Kills { get; set; }
    }
}

