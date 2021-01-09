namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15ce8cec3afL)]
    public class EnterBattleAsSpectatorFromLobbyRequestEvent : Event
    {
        public EnterBattleAsSpectatorFromLobbyRequestEvent(long battleId)
        {
            this.BattleId = battleId;
        }

        public long BattleId { get; set; }
    }
}

