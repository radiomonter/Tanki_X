namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x1199)]
    public struct HTML_URLChanged_t
    {
        public const int k_iCallback = 0x1199;
        public HHTMLBrowser unBrowserHandle;
        public string pchURL;
        public string pchPostData;
        [MarshalAs(UnmanagedType.I1)]
        public bool bIsRedirect;
        public string pchPageTitle;
        [MarshalAs(UnmanagedType.I1)]
        public bool bNewNavigation;
    }
}

