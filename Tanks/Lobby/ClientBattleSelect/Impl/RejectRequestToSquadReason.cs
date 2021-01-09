namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;

    public enum RejectRequestToSquadReason : byte
    {
        SQUAD_IS_FULL = 0,
        RECEIVER_REJECTED = 1
    }
}

