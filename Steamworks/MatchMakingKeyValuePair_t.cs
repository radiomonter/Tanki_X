namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct MatchMakingKeyValuePair_t
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x100)]
        public string m_szKey;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x100)]
        public string m_szValue;
        private MatchMakingKeyValuePair_t(string strKey, string strValue)
        {
            this.m_szKey = strKey;
            this.m_szValue = strValue;
        }
    }
}

