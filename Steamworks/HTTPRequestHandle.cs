namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct HTTPRequestHandle : IEquatable<HTTPRequestHandle>, IComparable<HTTPRequestHandle>
    {
        public static readonly HTTPRequestHandle Invalid;
        public uint m_HTTPRequestHandle;
        public HTTPRequestHandle(uint value)
        {
            this.m_HTTPRequestHandle = value;
        }

        public override string ToString() => 
            this.m_HTTPRequestHandle.ToString();

        public override bool Equals(object other) => 
            (other is HTTPRequestHandle) && (this == ((HTTPRequestHandle) other));

        public override int GetHashCode() => 
            this.m_HTTPRequestHandle.GetHashCode();

        public static bool operator ==(HTTPRequestHandle x, HTTPRequestHandle y) => 
            x.m_HTTPRequestHandle == y.m_HTTPRequestHandle;

        public static bool operator !=(HTTPRequestHandle x, HTTPRequestHandle y) => 
            !(x == y);

        public static explicit operator HTTPRequestHandle(uint value) => 
            new HTTPRequestHandle(value);

        public static explicit operator uint(HTTPRequestHandle that) => 
            that.m_HTTPRequestHandle;

        public bool Equals(HTTPRequestHandle other) => 
            this.m_HTTPRequestHandle == other.m_HTTPRequestHandle;

        public int CompareTo(HTTPRequestHandle other) => 
            this.m_HTTPRequestHandle.CompareTo(other.m_HTTPRequestHandle);

        static HTTPRequestHandle()
        {
            Invalid = new HTTPRequestHandle(0);
        }
    }
}

