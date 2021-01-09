namespace Steamworks
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void SteamAPI_CheckCallbackRegistered_t(int iCallbackNum);
}

