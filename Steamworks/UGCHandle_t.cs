namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct UGCHandle_t : IEquatable<UGCHandle_t>, IComparable<UGCHandle_t>
    {
        public static readonly UGCHandle_t Invalid;
        public ulong m_UGCHandle;
        public UGCHandle_t(ulong value)
        {
            this.m_UGCHandle = value;
        }

        public override string ToString() => 
            this.m_UGCHandle.ToString();

        public override bool Equals(object other) => 
            (other is UGCHandle_t) && (this == ((UGCHandle_t) other));

        public override int GetHashCode() => 
            this.m_UGCHandle.GetHashCode();

        public static bool operator ==(UGCHandle_t x, UGCHandle_t y) => 
            x.m_UGCHandle == y.m_UGCHandle;

        public static bool operator !=(UGCHandle_t x, UGCHandle_t y) => 
            !(x == y);

        public static explicit operator UGCHandle_t(ulong value) => 
            new UGCHandle_t(value);

        public static explicit operator ulong(UGCHandle_t that) => 
            that.m_UGCHandle;

        public bool Equals(UGCHandle_t other) => 
            this.m_UGCHandle == other.m_UGCHandle;

        public int CompareTo(UGCHandle_t other) => 
            this.m_UGCHandle.CompareTo(other.m_UGCHandle);

        static UGCHandle_t()
        {
            Invalid = new UGCHandle_t(ulong.MaxValue);
        }
    }
}

