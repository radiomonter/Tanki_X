namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x15a45b027a5L), Shared]
    public class SpiderMineConfigComponent : Component
    {
        public float Speed { get; set; }

        public float Acceleration { get; set; }

        public float Energy { get; set; }

        public float IdleEnergyDrainRate { get; set; }

        public float ChasingEnergyDrainRate { get; set; }
    }
}

