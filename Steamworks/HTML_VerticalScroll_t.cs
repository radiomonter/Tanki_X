namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x11a0)]
    public struct HTML_VerticalScroll_t
    {
        public const int k_iCallback = 0x11a0;
        public HHTMLBrowser unBrowserHandle;
        public uint unScrollMax;
        public uint unScrollCurrent;
        public float flPageScale;
        [MarshalAs(UnmanagedType.I1)]
        public bool bVisible;
        public uint unPageSize;
    }
}

