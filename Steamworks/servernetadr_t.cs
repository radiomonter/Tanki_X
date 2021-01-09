namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct servernetadr_t
    {
        private ushort m_usConnectionPort;
        private ushort m_usQueryPort;
        private uint m_unIP;
        public void Init(uint ip, ushort usQueryPort, ushort usConnectionPort)
        {
            this.m_unIP = ip;
            this.m_usQueryPort = usQueryPort;
            this.m_usConnectionPort = usConnectionPort;
        }

        public ushort GetQueryPort() => 
            this.m_usQueryPort;

        public void SetQueryPort(ushort usPort)
        {
            this.m_usQueryPort = usPort;
        }

        public ushort GetConnectionPort() => 
            this.m_usConnectionPort;

        public void SetConnectionPort(ushort usPort)
        {
            this.m_usConnectionPort = usPort;
        }

        public uint GetIP() => 
            this.m_unIP;

        public void SetIP(uint unIP)
        {
            this.m_unIP = unIP;
        }

        public string GetConnectionAddressString() => 
            ToString(this.m_unIP, this.m_usConnectionPort);

        public string GetQueryAddressString() => 
            ToString(this.m_unIP, this.m_usQueryPort);

        public static string ToString(uint unIP, ushort usPort) => 
            $"{(unIP >> 0x18) & 0xffL}.{(unIP >> 0x10) & 0xffL}.{(unIP >> 8) & 0xffL}.{unIP & 0xffL}:{usPort}";

        public static bool operator <(servernetadr_t x, servernetadr_t y) => 
            (x.m_unIP < y.m_unIP) || ((x.m_unIP == y.m_unIP) && (x.m_usQueryPort < y.m_usQueryPort));

        public static bool operator >(servernetadr_t x, servernetadr_t y) => 
            (x.m_unIP > y.m_unIP) || ((x.m_unIP == y.m_unIP) && (x.m_usQueryPort > y.m_usQueryPort));

        public override bool Equals(object other) => 
            (other is servernetadr_t) && (this == ((servernetadr_t) other));

        public override int GetHashCode() => 
            (this.m_unIP.GetHashCode() + this.m_usQueryPort.GetHashCode()) + this.m_usConnectionPort.GetHashCode();

        public static bool operator ==(servernetadr_t x, servernetadr_t y) => 
            ((x.m_unIP == y.m_unIP) && (x.m_usQueryPort == y.m_usQueryPort)) && (x.m_usConnectionPort == y.m_usConnectionPort);

        public static bool operator !=(servernetadr_t x, servernetadr_t y) => 
            !(x == y);

        public bool Equals(servernetadr_t other) => 
            ((this.m_unIP == other.m_unIP) && (this.m_usQueryPort == other.m_usQueryPort)) && (this.m_usConnectionPort == other.m_usConnectionPort);

        public int CompareTo(servernetadr_t other) => 
            (this.m_unIP.CompareTo(other.m_unIP) + this.m_usQueryPort.CompareTo(other.m_usQueryPort)) + this.m_usConnectionPort.CompareTo(other.m_usConnectionPort);
    }
}

