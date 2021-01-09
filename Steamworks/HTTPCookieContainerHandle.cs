namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct HTTPCookieContainerHandle : IEquatable<HTTPCookieContainerHandle>, IComparable<HTTPCookieContainerHandle>
    {
        public static readonly HTTPCookieContainerHandle Invalid;
        public uint m_HTTPCookieContainerHandle;
        public HTTPCookieContainerHandle(uint value)
        {
            this.m_HTTPCookieContainerHandle = value;
        }

        public override string ToString() => 
            this.m_HTTPCookieContainerHandle.ToString();

        public override bool Equals(object other) => 
            (other is HTTPCookieContainerHandle) && (this == ((HTTPCookieContainerHandle) other));

        public override int GetHashCode() => 
            this.m_HTTPCookieContainerHandle.GetHashCode();

        public static bool operator ==(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y) => 
            x.m_HTTPCookieContainerHandle == y.m_HTTPCookieContainerHandle;

        public static bool operator !=(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y) => 
            !(x == y);

        public static explicit operator HTTPCookieContainerHandle(uint value) => 
            new HTTPCookieContainerHandle(value);

        public static explicit operator uint(HTTPCookieContainerHandle that) => 
            that.m_HTTPCookieContainerHandle;

        public bool Equals(HTTPCookieContainerHandle other) => 
            this.m_HTTPCookieContainerHandle == other.m_HTTPCookieContainerHandle;

        public int CompareTo(HTTPCookieContainerHandle other) => 
            this.m_HTTPCookieContainerHandle.CompareTo(other.m_HTTPCookieContainerHandle);

        static HTTPCookieContainerHandle()
        {
            Invalid = new HTTPCookieContainerHandle(0);
        }
    }
}

