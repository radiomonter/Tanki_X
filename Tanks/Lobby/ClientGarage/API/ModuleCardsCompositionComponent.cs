namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4a99987540c11L)]
    public class ModuleCardsCompositionComponent : Component
    {
        public ModulePrice CraftPrice { get; set; }

        public List<ModulePrice> UpgradePrices { get; set; }
    }
}

