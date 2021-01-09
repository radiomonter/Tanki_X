﻿namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct SNetSocket_t : IEquatable<SNetSocket_t>, IComparable<SNetSocket_t>
    {
        public uint m_SNetSocket;
        public SNetSocket_t(uint value)
        {
            this.m_SNetSocket = value;
        }

        public override string ToString() => 
            this.m_SNetSocket.ToString();

        public override bool Equals(object other) => 
            (other is SNetSocket_t) && (this == ((SNetSocket_t) other));

        public override int GetHashCode() => 
            this.m_SNetSocket.GetHashCode();

        public static bool operator ==(SNetSocket_t x, SNetSocket_t y) => 
            x.m_SNetSocket == y.m_SNetSocket;

        public static bool operator !=(SNetSocket_t x, SNetSocket_t y) => 
            !(x == y);

        public static explicit operator SNetSocket_t(uint value) => 
            new SNetSocket_t(value);

        public static explicit operator uint(SNetSocket_t that) => 
            that.m_SNetSocket;

        public bool Equals(SNetSocket_t other) => 
            this.m_SNetSocket == other.m_SNetSocket;

        public int CompareTo(SNetSocket_t other) => 
            this.m_SNetSocket.CompareTo(other.m_SNetSocket);
    }
}

