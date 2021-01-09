namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct UGCQueryHandle_t : IEquatable<UGCQueryHandle_t>, IComparable<UGCQueryHandle_t>
    {
        public static readonly UGCQueryHandle_t Invalid;
        public ulong m_UGCQueryHandle;
        public UGCQueryHandle_t(ulong value)
        {
            this.m_UGCQueryHandle = value;
        }

        public override string ToString() => 
            this.m_UGCQueryHandle.ToString();

        public override bool Equals(object other) => 
            (other is UGCQueryHandle_t) && (this == ((UGCQueryHandle_t) other));

        public override int GetHashCode() => 
            this.m_UGCQueryHandle.GetHashCode();

        public static bool operator ==(UGCQueryHandle_t x, UGCQueryHandle_t y) => 
            x.m_UGCQueryHandle == y.m_UGCQueryHandle;

        public static bool operator !=(UGCQueryHandle_t x, UGCQueryHandle_t y) => 
            !(x == y);

        public static explicit operator UGCQueryHandle_t(ulong value) => 
            new UGCQueryHandle_t(value);

        public static explicit operator ulong(UGCQueryHandle_t that) => 
            that.m_UGCQueryHandle;

        public bool Equals(UGCQueryHandle_t other) => 
            this.m_UGCQueryHandle == other.m_UGCQueryHandle;

        public int CompareTo(UGCQueryHandle_t other) => 
            this.m_UGCQueryHandle.CompareTo(other.m_UGCQueryHandle);

        static UGCQueryHandle_t()
        {
            Invalid = new UGCQueryHandle_t(ulong.MaxValue);
        }
    }
}

