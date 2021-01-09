namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct HAuthTicket : IEquatable<HAuthTicket>, IComparable<HAuthTicket>
    {
        public static readonly HAuthTicket Invalid;
        public uint m_HAuthTicket;
        public HAuthTicket(uint value)
        {
            this.m_HAuthTicket = value;
        }

        public override string ToString() => 
            this.m_HAuthTicket.ToString();

        public override bool Equals(object other) => 
            (other is HAuthTicket) && (this == ((HAuthTicket) other));

        public override int GetHashCode() => 
            this.m_HAuthTicket.GetHashCode();

        public static bool operator ==(HAuthTicket x, HAuthTicket y) => 
            x.m_HAuthTicket == y.m_HAuthTicket;

        public static bool operator !=(HAuthTicket x, HAuthTicket y) => 
            !(x == y);

        public static explicit operator HAuthTicket(uint value) => 
            new HAuthTicket(value);

        public static explicit operator uint(HAuthTicket that) => 
            that.m_HAuthTicket;

        public bool Equals(HAuthTicket other) => 
            this.m_HAuthTicket == other.m_HAuthTicket;

        public int CompareTo(HAuthTicket other) => 
            this.m_HAuthTicket.CompareTo(other.m_HAuthTicket);

        static HAuthTicket()
        {
            Invalid = new HAuthTicket(0);
        }
    }
}

