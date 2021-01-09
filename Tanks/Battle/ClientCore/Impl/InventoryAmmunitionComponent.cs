namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4e2fac4f061a1L)]
    public class InventoryAmmunitionComponent : Component
    {
        public int MaxCount { get; set; }

        public int CurrentCount { get; set; }
    }
}

