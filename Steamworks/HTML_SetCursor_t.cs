namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x11aa)]
    public struct HTML_SetCursor_t
    {
        public const int k_iCallback = 0x11aa;
        public HHTMLBrowser unBrowserHandle;
        public uint eMouseCursor;
    }
}

