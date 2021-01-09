namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15f9d40d4dcL)]
    public class BattleEntrancePayerComponent : Component
    {
        public Dictionary<long, long> EnergyPayments { get; set; }
    }
}

