namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x11ab)]
    public struct HTML_StatusText_t
    {
        public const int k_iCallback = 0x11ab;
        public HHTMLBrowser unBrowserHandle;
        public string pchMsg;
    }
}

