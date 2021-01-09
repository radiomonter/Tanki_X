namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x5ae5b51875e8d26eL)]
    public class BattleTankCollisionsComponent : Component
    {
        public long SemiActiveCollisionsPhase { get; set; }
    }
}

