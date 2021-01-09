namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x119d)]
    public struct HTML_SearchResults_t
    {
        public const int k_iCallback = 0x119d;
        public HHTMLBrowser unBrowserHandle;
        public uint unResults;
        public uint unCurrentMatch;
    }
}

