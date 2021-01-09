namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x119b)]
    public struct HTML_OpenLinkInNewTab_t
    {
        public const int k_iCallback = 0x119b;
        public HHTMLBrowser unBrowserHandle;
        public string pchURL;
    }
}

