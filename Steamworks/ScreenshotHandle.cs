namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct ScreenshotHandle : IEquatable<ScreenshotHandle>, IComparable<ScreenshotHandle>
    {
        public static readonly ScreenshotHandle Invalid;
        public uint m_ScreenshotHandle;
        public ScreenshotHandle(uint value)
        {
            this.m_ScreenshotHandle = value;
        }

        public override string ToString() => 
            this.m_ScreenshotHandle.ToString();

        public override bool Equals(object other) => 
            (other is ScreenshotHandle) && (this == ((ScreenshotHandle) other));

        public override int GetHashCode() => 
            this.m_ScreenshotHandle.GetHashCode();

        public static bool operator ==(ScreenshotHandle x, ScreenshotHandle y) => 
            x.m_ScreenshotHandle == y.m_ScreenshotHandle;

        public static bool operator !=(ScreenshotHandle x, ScreenshotHandle y) => 
            !(x == y);

        public static explicit operator ScreenshotHandle(uint value) => 
            new ScreenshotHandle(value);

        public static explicit operator uint(ScreenshotHandle that) => 
            that.m_ScreenshotHandle;

        public bool Equals(ScreenshotHandle other) => 
            this.m_ScreenshotHandle == other.m_ScreenshotHandle;

        public int CompareTo(ScreenshotHandle other) => 
            this.m_ScreenshotHandle.CompareTo(other.m_ScreenshotHandle);

        static ScreenshotHandle()
        {
            Invalid = new ScreenshotHandle(0);
        }
    }
}

