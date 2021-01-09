namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0x2ca)]
    public struct GamepadTextInputDismissed_t
    {
        public const int k_iCallback = 0x2ca;
        [MarshalAs(UnmanagedType.I1)]
        public bool m_bSubmitted;
        public uint m_unSubmittedText;
    }
}

