namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct HHTMLBrowser : IEquatable<HHTMLBrowser>, IComparable<HHTMLBrowser>
    {
        public static readonly HHTMLBrowser Invalid;
        public uint m_HHTMLBrowser;
        public HHTMLBrowser(uint value)
        {
            this.m_HHTMLBrowser = value;
        }

        public override string ToString() => 
            this.m_HHTMLBrowser.ToString();

        public override bool Equals(object other) => 
            (other is HHTMLBrowser) && (this == ((HHTMLBrowser) other));

        public override int GetHashCode() => 
            this.m_HHTMLBrowser.GetHashCode();

        public static bool operator ==(HHTMLBrowser x, HHTMLBrowser y) => 
            x.m_HHTMLBrowser == y.m_HHTMLBrowser;

        public static bool operator !=(HHTMLBrowser x, HHTMLBrowser y) => 
            !(x == y);

        public static explicit operator HHTMLBrowser(uint value) => 
            new HHTMLBrowser(value);

        public static explicit operator uint(HHTMLBrowser that) => 
            that.m_HHTMLBrowser;

        public bool Equals(HHTMLBrowser other) => 
            this.m_HHTMLBrowser == other.m_HHTMLBrowser;

        public int CompareTo(HHTMLBrowser other) => 
            this.m_HHTMLBrowser.CompareTo(other.m_HHTMLBrowser);

        static HHTMLBrowser()
        {
            Invalid = new HHTMLBrowser(0);
        }
    }
}

