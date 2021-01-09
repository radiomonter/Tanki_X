namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct AppId_t : IEquatable<AppId_t>, IComparable<AppId_t>
    {
        public static readonly AppId_t Invalid;
        public uint m_AppId;
        public AppId_t(uint value)
        {
            this.m_AppId = value;
        }

        public override string ToString() => 
            this.m_AppId.ToString();

        public override bool Equals(object other) => 
            (other is AppId_t) && (this == ((AppId_t) other));

        public override int GetHashCode() => 
            this.m_AppId.GetHashCode();

        public static bool operator ==(AppId_t x, AppId_t y) => 
            x.m_AppId == y.m_AppId;

        public static bool operator !=(AppId_t x, AppId_t y) => 
            !(x == y);

        public static explicit operator AppId_t(uint value) => 
            new AppId_t(value);

        public static explicit operator uint(AppId_t that) => 
            that.m_AppId;

        public bool Equals(AppId_t other) => 
            this.m_AppId == other.m_AppId;

        public int CompareTo(AppId_t other) => 
            this.m_AppId.CompareTo(other.m_AppId);

        static AppId_t()
        {
            Invalid = new AppId_t(0);
        }
    }
}

