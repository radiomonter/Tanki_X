namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct HServerListRequest : IEquatable<HServerListRequest>
    {
        public static readonly HServerListRequest Invalid;
        public IntPtr m_HServerListRequest;
        public HServerListRequest(IntPtr value)
        {
            this.m_HServerListRequest = value;
        }

        public override string ToString() => 
            this.m_HServerListRequest.ToString();

        public override bool Equals(object other) => 
            (other is HServerListRequest) && (this == ((HServerListRequest) other));

        public override int GetHashCode() => 
            this.m_HServerListRequest.GetHashCode();

        public static bool operator ==(HServerListRequest x, HServerListRequest y) => 
            x.m_HServerListRequest == y.m_HServerListRequest;

        public static bool operator !=(HServerListRequest x, HServerListRequest y) => 
            !(x == y);

        public static explicit operator HServerListRequest(IntPtr value) => 
            new HServerListRequest(value);

        public static explicit operator IntPtr(HServerListRequest that) => 
            that.m_HServerListRequest;

        public bool Equals(HServerListRequest other) => 
            this.m_HServerListRequest == other.m_HServerListRequest;

        static HServerListRequest()
        {
            Invalid = new HServerListRequest(IntPtr.Zero);
        }
    }
}

