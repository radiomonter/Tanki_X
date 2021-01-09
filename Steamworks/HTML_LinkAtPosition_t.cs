namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x11a1)]
    public struct HTML_LinkAtPosition_t
    {
        public const int k_iCallback = 0x11a1;
        public HHTMLBrowser unBrowserHandle;
        public uint x;
        public uint y;
        public string pchURL;
        [MarshalAs(UnmanagedType.I1)]
        public bool bInput;
        [MarshalAs(UnmanagedType.I1)]
        public bool bLiveLink;
    }
}

