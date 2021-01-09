namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    [Shared, SerialVersionUID(0x15e3187f2a6L)]
    public class ElevatedAccessUserIncreaseScoreEvent : Event
    {
        public Tanks.Battle.ClientCore.API.TeamColor TeamColor { get; set; }

        public int Amount { get; set; }
    }
}

