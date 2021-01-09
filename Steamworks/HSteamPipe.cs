namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct HSteamPipe : IEquatable<HSteamPipe>, IComparable<HSteamPipe>
    {
        public int m_HSteamPipe;
        public HSteamPipe(int value)
        {
            this.m_HSteamPipe = value;
        }

        public override string ToString() => 
            this.m_HSteamPipe.ToString();

        public override bool Equals(object other) => 
            (other is HSteamPipe) && (this == ((HSteamPipe) other));

        public override int GetHashCode() => 
            this.m_HSteamPipe.GetHashCode();

        public static bool operator ==(HSteamPipe x, HSteamPipe y) => 
            x.m_HSteamPipe == y.m_HSteamPipe;

        public static bool operator !=(HSteamPipe x, HSteamPipe y) => 
            !(x == y);

        public static explicit operator HSteamPipe(int value) => 
            new HSteamPipe(value);

        public static explicit operator int(HSteamPipe that) => 
            that.m_HSteamPipe;

        public bool Equals(HSteamPipe other) => 
            this.m_HSteamPipe == other.m_HSteamPipe;

        public int CompareTo(HSteamPipe other) => 
            this.m_HSteamPipe.CompareTo(other.m_HSteamPipe);
    }
}

