namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-6990482179466020052L)]
    public class TankDamageHistoryComponent : Component
    {
        public List<DamageHistoryItem> DamageHistory { get; set; }
    }
}

