namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct AccountID_t : IEquatable<AccountID_t>, IComparable<AccountID_t>
    {
        public uint m_AccountID;
        public AccountID_t(uint value)
        {
            this.m_AccountID = value;
        }

        public override string ToString() => 
            this.m_AccountID.ToString();

        public override bool Equals(object other) => 
            (other is AccountID_t) && (this == ((AccountID_t) other));

        public override int GetHashCode() => 
            this.m_AccountID.GetHashCode();

        public static bool operator ==(AccountID_t x, AccountID_t y) => 
            x.m_AccountID == y.m_AccountID;

        public static bool operator !=(AccountID_t x, AccountID_t y) => 
            !(x == y);

        public static explicit operator AccountID_t(uint value) => 
            new AccountID_t(value);

        public static explicit operator uint(AccountID_t that) => 
            that.m_AccountID;

        public bool Equals(AccountID_t other) => 
            this.m_AccountID == other.m_AccountID;

        public int CompareTo(AccountID_t other) => 
            this.m_AccountID.CompareTo(other.m_AccountID);
    }
}

