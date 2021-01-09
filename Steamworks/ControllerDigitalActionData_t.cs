namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct ControllerDigitalActionData_t
    {
        public byte bState;
        public byte bActive;
    }
}

