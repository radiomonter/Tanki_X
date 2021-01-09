namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d4b24b9b1e02b0L)]
    public class ModuleUpgradablePowerConfigComponent : Component
    {
        public List<List<int>> Level2PowerByTier { get; set; }
    }
}

