namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct SNetListenSocket_t : IEquatable<SNetListenSocket_t>, IComparable<SNetListenSocket_t>
    {
        public uint m_SNetListenSocket;
        public SNetListenSocket_t(uint value)
        {
            this.m_SNetListenSocket = value;
        }

        public override string ToString() => 
            this.m_SNetListenSocket.ToString();

        public override bool Equals(object other) => 
            (other is SNetListenSocket_t) && (this == ((SNetListenSocket_t) other));

        public override int GetHashCode() => 
            this.m_SNetListenSocket.GetHashCode();

        public static bool operator ==(SNetListenSocket_t x, SNetListenSocket_t y) => 
            x.m_SNetListenSocket == y.m_SNetListenSocket;

        public static bool operator !=(SNetListenSocket_t x, SNetListenSocket_t y) => 
            !(x == y);

        public static explicit operator SNetListenSocket_t(uint value) => 
            new SNetListenSocket_t(value);

        public static explicit operator uint(SNetListenSocket_t that) => 
            that.m_SNetListenSocket;

        public bool Equals(SNetListenSocket_t other) => 
            this.m_SNetListenSocket == other.m_SNetListenSocket;

        public int CompareTo(SNetListenSocket_t other) => 
            this.m_SNetListenSocket.CompareTo(other.m_SNetListenSocket);
    }
}

