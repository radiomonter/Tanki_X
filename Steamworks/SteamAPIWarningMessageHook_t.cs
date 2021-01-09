namespace Steamworks
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SteamAPIWarningMessageHook_t(int nSeverity, StringBuilder pchDebugText);
}

