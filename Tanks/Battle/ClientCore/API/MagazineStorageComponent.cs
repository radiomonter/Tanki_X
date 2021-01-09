namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x2124b9076f21734fL)]
    public class MagazineStorageComponent : Component
    {
        public int CurrentCartridgeCount { get; set; }
    }
}

