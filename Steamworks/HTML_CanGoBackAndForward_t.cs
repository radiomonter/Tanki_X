namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x119e)]
    public struct HTML_CanGoBackAndForward_t
    {
        public const int k_iCallback = 0x119e;
        public HHTMLBrowser unBrowserHandle;
        [MarshalAs(UnmanagedType.I1)]
        public bool bCanGoBack;
        [MarshalAs(UnmanagedType.I1)]
        public bool bCanGoForward;
    }
}

