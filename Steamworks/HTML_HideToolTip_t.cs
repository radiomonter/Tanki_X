namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x11ae)]
    public struct HTML_HideToolTip_t
    {
        public const int k_iCallback = 0x11ae;
        public HHTMLBrowser unBrowserHandle;
    }
}

