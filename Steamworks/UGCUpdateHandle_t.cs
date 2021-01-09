namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct UGCUpdateHandle_t : IEquatable<UGCUpdateHandle_t>, IComparable<UGCUpdateHandle_t>
    {
        public static readonly UGCUpdateHandle_t Invalid;
        public ulong m_UGCUpdateHandle;
        public UGCUpdateHandle_t(ulong value)
        {
            this.m_UGCUpdateHandle = value;
        }

        public override string ToString() => 
            this.m_UGCUpdateHandle.ToString();

        public override bool Equals(object other) => 
            (other is UGCUpdateHandle_t) && (this == ((UGCUpdateHandle_t) other));

        public override int GetHashCode() => 
            this.m_UGCUpdateHandle.GetHashCode();

        public static bool operator ==(UGCUpdateHandle_t x, UGCUpdateHandle_t y) => 
            x.m_UGCUpdateHandle == y.m_UGCUpdateHandle;

        public static bool operator !=(UGCUpdateHandle_t x, UGCUpdateHandle_t y) => 
            !(x == y);

        public static explicit operator UGCUpdateHandle_t(ulong value) => 
            new UGCUpdateHandle_t(value);

        public static explicit operator ulong(UGCUpdateHandle_t that) => 
            that.m_UGCUpdateHandle;

        public bool Equals(UGCUpdateHandle_t other) => 
            this.m_UGCUpdateHandle == other.m_UGCUpdateHandle;

        public int CompareTo(UGCUpdateHandle_t other) => 
            this.m_UGCUpdateHandle.CompareTo(other.m_UGCUpdateHandle);

        static UGCUpdateHandle_t()
        {
            Invalid = new UGCUpdateHandle_t(ulong.MaxValue);
        }
    }
}

