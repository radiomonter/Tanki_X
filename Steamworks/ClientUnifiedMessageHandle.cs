namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct ClientUnifiedMessageHandle : IEquatable<ClientUnifiedMessageHandle>, IComparable<ClientUnifiedMessageHandle>
    {
        public static readonly ClientUnifiedMessageHandle Invalid;
        public ulong m_ClientUnifiedMessageHandle;
        public ClientUnifiedMessageHandle(ulong value)
        {
            this.m_ClientUnifiedMessageHandle = value;
        }

        public override string ToString() => 
            this.m_ClientUnifiedMessageHandle.ToString();

        public override bool Equals(object other) => 
            (other is ClientUnifiedMessageHandle) && (this == ((ClientUnifiedMessageHandle) other));

        public override int GetHashCode() => 
            this.m_ClientUnifiedMessageHandle.GetHashCode();

        public static bool operator ==(ClientUnifiedMessageHandle x, ClientUnifiedMessageHandle y) => 
            x.m_ClientUnifiedMessageHandle == y.m_ClientUnifiedMessageHandle;

        public static bool operator !=(ClientUnifiedMessageHandle x, ClientUnifiedMessageHandle y) => 
            !(x == y);

        public static explicit operator ClientUnifiedMessageHandle(ulong value) => 
            new ClientUnifiedMessageHandle(value);

        public static explicit operator ulong(ClientUnifiedMessageHandle that) => 
            that.m_ClientUnifiedMessageHandle;

        public bool Equals(ClientUnifiedMessageHandle other) => 
            this.m_ClientUnifiedMessageHandle == other.m_ClientUnifiedMessageHandle;

        public int CompareTo(ClientUnifiedMessageHandle other) => 
            this.m_ClientUnifiedMessageHandle.CompareTo(other.m_ClientUnifiedMessageHandle);

        static ClientUnifiedMessageHandle()
        {
            Invalid = new ClientUnifiedMessageHandle(0L);
        }
    }
}

