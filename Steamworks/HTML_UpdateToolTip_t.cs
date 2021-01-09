namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x11ad)]
    public struct HTML_UpdateToolTip_t
    {
        public const int k_iCallback = 0x11ad;
        public HHTMLBrowser unBrowserHandle;
        public string pchMsg;
    }
}

