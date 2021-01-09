namespace Steamworks
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal class CCallbackBaseVTable
    {
        private const CallingConvention cc = CallingConvention.StdCall;
        [NonSerialized, MarshalAs(UnmanagedType.FunctionPtr)]
        public RunCRDel m_RunCallResult;
        [NonSerialized, MarshalAs(UnmanagedType.FunctionPtr)]
        public RunCBDel m_RunCallback;
        [NonSerialized, MarshalAs(UnmanagedType.FunctionPtr)]
        public GetCallbackSizeBytesDel m_GetCallbackSizeBytes;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int GetCallbackSizeBytesDel();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void RunCBDel(IntPtr pvParam);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void RunCRDel(IntPtr pvParam, [MarshalAs(UnmanagedType.I1)] bool bIOFailure, ulong hSteamAPICall);
    }
}

