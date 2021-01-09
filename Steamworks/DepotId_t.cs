namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct DepotId_t : IEquatable<DepotId_t>, IComparable<DepotId_t>
    {
        public static readonly DepotId_t Invalid;
        public uint m_DepotId;
        public DepotId_t(uint value)
        {
            this.m_DepotId = value;
        }

        public override string ToString() => 
            this.m_DepotId.ToString();

        public override bool Equals(object other) => 
            (other is DepotId_t) && (this == ((DepotId_t) other));

        public override int GetHashCode() => 
            this.m_DepotId.GetHashCode();

        public static bool operator ==(DepotId_t x, DepotId_t y) => 
            x.m_DepotId == y.m_DepotId;

        public static bool operator !=(DepotId_t x, DepotId_t y) => 
            !(x == y);

        public static explicit operator DepotId_t(uint value) => 
            new DepotId_t(value);

        public static explicit operator uint(DepotId_t that) => 
            that.m_DepotId;

        public bool Equals(DepotId_t other) => 
            this.m_DepotId == other.m_DepotId;

        public int CompareTo(DepotId_t other) => 
            this.m_DepotId.CompareTo(other.m_DepotId);

        static DepotId_t()
        {
            Invalid = new DepotId_t(0);
        }
    }
}

