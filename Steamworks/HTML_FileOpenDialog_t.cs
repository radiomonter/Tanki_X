namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x11a4)]
    public struct HTML_FileOpenDialog_t
    {
        public const int k_iCallback = 0x11a4;
        public HHTMLBrowser unBrowserHandle;
        public string pchTitle;
        public string pchInitialFile;
    }
}

