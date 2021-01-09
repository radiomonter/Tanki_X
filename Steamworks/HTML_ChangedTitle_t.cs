namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x119c)]
    public struct HTML_ChangedTitle_t
    {
        public const int k_iCallback = 0x119c;
        public HHTMLBrowser unBrowserHandle;
        public string pchTitle;
    }
}

