namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1524044252bL)]
    public class RequestBattleEvent : Event
    {
        public RequestBattleEvent()
        {
        }

        public RequestBattleEvent(long battleId)
        {
            this.BattleId = battleId;
        }

        public long BattleId { get; set; }
    }
}

