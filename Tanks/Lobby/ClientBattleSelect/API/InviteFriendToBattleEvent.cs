namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d32be7c368d8acL)]
    public class InviteFriendToBattleEvent : Event
    {
        public InviteFriendToBattleEvent(long battleId)
        {
            this.BattleId = battleId;
        }

        public long BattleId { get; set; }
    }
}

