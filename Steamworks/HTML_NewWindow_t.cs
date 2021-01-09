namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x11a9)]
    public struct HTML_NewWindow_t
    {
        public const int k_iCallback = 0x11a9;
        public HHTMLBrowser unBrowserHandle;
        public string pchURL;
        public uint unX;
        public uint unY;
        public uint unWide;
        public uint unTall;
        public HHTMLBrowser unNewWindow_BrowserHandle;
    }
}

