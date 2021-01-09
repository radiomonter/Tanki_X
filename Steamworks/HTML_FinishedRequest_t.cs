namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x119a)]
    public struct HTML_FinishedRequest_t
    {
        public const int k_iCallback = 0x119a;
        public HHTMLBrowser unBrowserHandle;
        public string pchURL;
        public string pchPageTitle;
    }
}

