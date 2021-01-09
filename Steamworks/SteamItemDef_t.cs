namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct SteamItemDef_t : IEquatable<SteamItemDef_t>, IComparable<SteamItemDef_t>
    {
        public int m_SteamItemDef;
        public SteamItemDef_t(int value)
        {
            this.m_SteamItemDef = value;
        }

        public override string ToString() => 
            this.m_SteamItemDef.ToString();

        public override bool Equals(object other) => 
            (other is SteamItemDef_t) && (this == ((SteamItemDef_t) other));

        public override int GetHashCode() => 
            this.m_SteamItemDef.GetHashCode();

        public static bool operator ==(SteamItemDef_t x, SteamItemDef_t y) => 
            x.m_SteamItemDef == y.m_SteamItemDef;

        public static bool operator !=(SteamItemDef_t x, SteamItemDef_t y) => 
            !(x == y);

        public static explicit operator SteamItemDef_t(int value) => 
            new SteamItemDef_t(value);

        public static explicit operator int(SteamItemDef_t that) => 
            that.m_SteamItemDef;

        public bool Equals(SteamItemDef_t other) => 
            this.m_SteamItemDef == other.m_SteamItemDef;

        public int CompareTo(SteamItemDef_t other) => 
            this.m_SteamItemDef.CompareTo(other.m_SteamItemDef);
    }
}

