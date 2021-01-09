namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4d40e659aa75fL)]
    public class BattleUserInventoryCooldownSpeedComponent : Component
    {
        public float SpeedCoeff { get; set; }
    }
}

