namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x11a3)]
    public struct HTML_JSConfirm_t
    {
        public const int k_iCallback = 0x11a3;
        public HHTMLBrowser unBrowserHandle;
        public string pchMessage;
    }
}

