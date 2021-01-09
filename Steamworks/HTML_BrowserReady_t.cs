namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x1195)]
    public struct HTML_BrowserReady_t
    {
        public const int k_iCallback = 0x1195;
        public HHTMLBrowser unBrowserHandle;
    }
}

