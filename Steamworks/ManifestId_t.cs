namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct ManifestId_t : IEquatable<ManifestId_t>, IComparable<ManifestId_t>
    {
        public static readonly ManifestId_t Invalid;
        public ulong m_ManifestId;
        public ManifestId_t(ulong value)
        {
            this.m_ManifestId = value;
        }

        public override string ToString() => 
            this.m_ManifestId.ToString();

        public override bool Equals(object other) => 
            (other is ManifestId_t) && (this == ((ManifestId_t) other));

        public override int GetHashCode() => 
            this.m_ManifestId.GetHashCode();

        public static bool operator ==(ManifestId_t x, ManifestId_t y) => 
            x.m_ManifestId == y.m_ManifestId;

        public static bool operator !=(ManifestId_t x, ManifestId_t y) => 
            !(x == y);

        public static explicit operator ManifestId_t(ulong value) => 
            new ManifestId_t(value);

        public static explicit operator ulong(ManifestId_t that) => 
            that.m_ManifestId;

        public bool Equals(ManifestId_t other) => 
            this.m_ManifestId == other.m_ManifestId;

        public int CompareTo(ManifestId_t other) => 
            this.m_ManifestId.CompareTo(other.m_ManifestId);

        static ManifestId_t()
        {
            Invalid = new ManifestId_t(0L);
        }
    }
}

