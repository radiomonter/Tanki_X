namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x11a2)]
    public struct HTML_JSAlert_t
    {
        public const int k_iCallback = 0x11a2;
        public HHTMLBrowser unBrowserHandle;
        public string pchMessage;
    }
}

