namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-3961778961585441606L)]
    public class BonusRegionComponent : Component
    {
        public BonusType Type { get; set; }
    }
}

