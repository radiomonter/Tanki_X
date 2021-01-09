namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d3233e6e3e44eaL)]
    public class RequestLoadBattleInfoEvent : Event
    {
        public RequestLoadBattleInfoEvent(long battleId)
        {
            this.BattleId = battleId;
        }

        public long BattleId { get; private set; }
    }
}

