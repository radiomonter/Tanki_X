namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct PublishedFileUpdateHandle_t : IEquatable<PublishedFileUpdateHandle_t>, IComparable<PublishedFileUpdateHandle_t>
    {
        public static readonly PublishedFileUpdateHandle_t Invalid;
        public ulong m_PublishedFileUpdateHandle;
        public PublishedFileUpdateHandle_t(ulong value)
        {
            this.m_PublishedFileUpdateHandle = value;
        }

        public override string ToString() => 
            this.m_PublishedFileUpdateHandle.ToString();

        public override bool Equals(object other) => 
            (other is PublishedFileUpdateHandle_t) && (this == ((PublishedFileUpdateHandle_t) other));

        public override int GetHashCode() => 
            this.m_PublishedFileUpdateHandle.GetHashCode();

        public static bool operator ==(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y) => 
            x.m_PublishedFileUpdateHandle == y.m_PublishedFileUpdateHandle;

        public static bool operator !=(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y) => 
            !(x == y);

        public static explicit operator PublishedFileUpdateHandle_t(ulong value) => 
            new PublishedFileUpdateHandle_t(value);

        public static explicit operator ulong(PublishedFileUpdateHandle_t that) => 
            that.m_PublishedFileUpdateHandle;

        public bool Equals(PublishedFileUpdateHandle_t other) => 
            this.m_PublishedFileUpdateHandle == other.m_PublishedFileUpdateHandle;

        public int CompareTo(PublishedFileUpdateHandle_t other) => 
            this.m_PublishedFileUpdateHandle.CompareTo(other.m_PublishedFileUpdateHandle);

        static PublishedFileUpdateHandle_t()
        {
            Invalid = new PublishedFileUpdateHandle_t(ulong.MaxValue);
        }
    }
}

