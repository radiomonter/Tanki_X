namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct FriendsGroupID_t : IEquatable<FriendsGroupID_t>, IComparable<FriendsGroupID_t>
    {
        public static readonly FriendsGroupID_t Invalid;
        public short m_FriendsGroupID;
        public FriendsGroupID_t(short value)
        {
            this.m_FriendsGroupID = value;
        }

        public override string ToString() => 
            this.m_FriendsGroupID.ToString();

        public override bool Equals(object other) => 
            (other is FriendsGroupID_t) && (this == ((FriendsGroupID_t) other));

        public override int GetHashCode() => 
            this.m_FriendsGroupID.GetHashCode();

        public static bool operator ==(FriendsGroupID_t x, FriendsGroupID_t y) => 
            x.m_FriendsGroupID == y.m_FriendsGroupID;

        public static bool operator !=(FriendsGroupID_t x, FriendsGroupID_t y) => 
            !(x == y);

        public static explicit operator FriendsGroupID_t(short value) => 
            new FriendsGroupID_t(value);

        public static explicit operator short(FriendsGroupID_t that) => 
            that.m_FriendsGroupID;

        public bool Equals(FriendsGroupID_t other) => 
            this.m_FriendsGroupID == other.m_FriendsGroupID;

        public int CompareTo(FriendsGroupID_t other) => 
            this.m_FriendsGroupID.CompareTo(other.m_FriendsGroupID);

        static FriendsGroupID_t()
        {
            Invalid = new FriendsGroupID_t(-1);
        }
    }
}

