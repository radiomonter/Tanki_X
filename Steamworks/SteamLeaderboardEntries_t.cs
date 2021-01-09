namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct SteamLeaderboardEntries_t : IEquatable<SteamLeaderboardEntries_t>, IComparable<SteamLeaderboardEntries_t>
    {
        public ulong m_SteamLeaderboardEntries;
        public SteamLeaderboardEntries_t(ulong value)
        {
            this.m_SteamLeaderboardEntries = value;
        }

        public override string ToString() => 
            this.m_SteamLeaderboardEntries.ToString();

        public override bool Equals(object other) => 
            (other is SteamLeaderboardEntries_t) && (this == ((SteamLeaderboardEntries_t) other));

        public override int GetHashCode() => 
            this.m_SteamLeaderboardEntries.GetHashCode();

        public static bool operator ==(SteamLeaderboardEntries_t x, SteamLeaderboardEntries_t y) => 
            x.m_SteamLeaderboardEntries == y.m_SteamLeaderboardEntries;

        public static bool operator !=(SteamLeaderboardEntries_t x, SteamLeaderboardEntries_t y) => 
            !(x == y);

        public static explicit operator SteamLeaderboardEntries_t(ulong value) => 
            new SteamLeaderboardEntries_t(value);

        public static explicit operator ulong(SteamLeaderboardEntries_t that) => 
            that.m_SteamLeaderboardEntries;

        public bool Equals(SteamLeaderboardEntries_t other) => 
            this.m_SteamLeaderboardEntries == other.m_SteamLeaderboardEntries;

        public int CompareTo(SteamLeaderboardEntries_t other) => 
            this.m_SteamLeaderboardEntries.CompareTo(other.m_SteamLeaderboardEntries);
    }
}

