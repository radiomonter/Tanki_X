namespace Steamworks
{
    using System;

    [Flags]
    public enum EChatSteamIDInstanceFlags
    {
        k_EChatAccountInstanceMask = 0xfff,
        k_EChatInstanceFlagClan = 0x80000,
        k_EChatInstanceFlagLobby = 0x40000,
        k_EChatInstanceFlagMMSLobby = 0x20000
    }
}

