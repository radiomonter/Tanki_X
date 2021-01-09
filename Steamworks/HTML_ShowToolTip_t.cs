namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x11ac)]
    public struct HTML_ShowToolTip_t
    {
        public const int k_iCallback = 0x11ac;
        public HHTMLBrowser unBrowserHandle;
        public string pchMsg;
    }
}

