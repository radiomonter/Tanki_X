namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x1197)]
    public struct HTML_StartRequest_t
    {
        public const int k_iCallback = 0x1197;
        public HHTMLBrowser unBrowserHandle;
        public string pchURL;
        public string pchTarget;
        public string pchPostData;
        [MarshalAs(UnmanagedType.I1)]
        public bool bIsRedirect;
    }
}

