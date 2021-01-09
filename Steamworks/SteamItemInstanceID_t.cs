namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct SteamItemInstanceID_t : IEquatable<SteamItemInstanceID_t>, IComparable<SteamItemInstanceID_t>
    {
        public static readonly SteamItemInstanceID_t Invalid;
        public ulong m_SteamItemInstanceID;
        public SteamItemInstanceID_t(ulong value)
        {
            this.m_SteamItemInstanceID = value;
        }

        public override string ToString() => 
            this.m_SteamItemInstanceID.ToString();

        public override bool Equals(object other) => 
            (other is SteamItemInstanceID_t) && (this == ((SteamItemInstanceID_t) other));

        public override int GetHashCode() => 
            this.m_SteamItemInstanceID.GetHashCode();

        public static bool operator ==(SteamItemInstanceID_t x, SteamItemInstanceID_t y) => 
            x.m_SteamItemInstanceID == y.m_SteamItemInstanceID;

        public static bool operator !=(SteamItemInstanceID_t x, SteamItemInstanceID_t y) => 
            !(x == y);

        public static explicit operator SteamItemInstanceID_t(ulong value) => 
            new SteamItemInstanceID_t(value);

        public static explicit operator ulong(SteamItemInstanceID_t that) => 
            that.m_SteamItemInstanceID;

        public bool Equals(SteamItemInstanceID_t other) => 
            this.m_SteamItemInstanceID == other.m_SteamItemInstanceID;

        public int CompareTo(SteamItemInstanceID_t other) => 
            this.m_SteamItemInstanceID.CompareTo(other.m_SteamItemInstanceID);

        static SteamItemInstanceID_t()
        {
            Invalid = new SteamItemInstanceID_t(ulong.MaxValue);
        }
    }
}

