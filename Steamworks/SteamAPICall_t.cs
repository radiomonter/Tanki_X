namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct SteamAPICall_t : IEquatable<SteamAPICall_t>, IComparable<SteamAPICall_t>
    {
        public static readonly SteamAPICall_t Invalid;
        public ulong m_SteamAPICall;
        public SteamAPICall_t(ulong value)
        {
            this.m_SteamAPICall = value;
        }

        public override string ToString() => 
            this.m_SteamAPICall.ToString();

        public override bool Equals(object other) => 
            (other is SteamAPICall_t) && (this == ((SteamAPICall_t) other));

        public override int GetHashCode() => 
            this.m_SteamAPICall.GetHashCode();

        public static bool operator ==(SteamAPICall_t x, SteamAPICall_t y) => 
            x.m_SteamAPICall == y.m_SteamAPICall;

        public static bool operator !=(SteamAPICall_t x, SteamAPICall_t y) => 
            !(x == y);

        public static explicit operator SteamAPICall_t(ulong value) => 
            new SteamAPICall_t(value);

        public static explicit operator ulong(SteamAPICall_t that) => 
            that.m_SteamAPICall;

        public bool Equals(SteamAPICall_t other) => 
            this.m_SteamAPICall == other.m_SteamAPICall;

        public int CompareTo(SteamAPICall_t other) => 
            this.m_SteamAPICall.CompareTo(other.m_SteamAPICall);

        static SteamAPICall_t()
        {
            Invalid = new SteamAPICall_t(0L);
        }
    }
}

