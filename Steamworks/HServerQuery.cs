namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct HServerQuery : IEquatable<HServerQuery>, IComparable<HServerQuery>
    {
        public static readonly HServerQuery Invalid;
        public int m_HServerQuery;
        public HServerQuery(int value)
        {
            this.m_HServerQuery = value;
        }

        public override string ToString() => 
            this.m_HServerQuery.ToString();

        public override bool Equals(object other) => 
            (other is HServerQuery) && (this == ((HServerQuery) other));

        public override int GetHashCode() => 
            this.m_HServerQuery.GetHashCode();

        public static bool operator ==(HServerQuery x, HServerQuery y) => 
            x.m_HServerQuery == y.m_HServerQuery;

        public static bool operator !=(HServerQuery x, HServerQuery y) => 
            !(x == y);

        public static explicit operator HServerQuery(int value) => 
            new HServerQuery(value);

        public static explicit operator int(HServerQuery that) => 
            that.m_HServerQuery;

        public bool Equals(HServerQuery other) => 
            this.m_HServerQuery == other.m_HServerQuery;

        public int CompareTo(HServerQuery other) => 
            this.m_HServerQuery.CompareTo(other.m_HServerQuery);

        static HServerQuery()
        {
            Invalid = new HServerQuery(-1);
        }
    }
}

