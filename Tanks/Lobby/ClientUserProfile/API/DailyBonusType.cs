namespace Tanks.Lobby.ClientUserProfile.API
{
    using System;

    [Flags]
    public enum DailyBonusType
    {
        NONE = 0,
        CRY = 1,
        XCRY = 2,
        ENERGY = 4,
        CONTAINER = 8,
        DETAIL = 0x10
    }
}

