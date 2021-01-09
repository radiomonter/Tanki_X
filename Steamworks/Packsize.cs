namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    public static class Packsize
    {
        public const int value = 8;

        public static bool Test()
        {
            int num2 = Marshal.SizeOf(typeof(RemoteStorageEnumerateUserSubscribedFilesResult_t));
            return ((Marshal.SizeOf(typeof(ValvePackingSentinel_t)) == 0x20) && (num2 == 0x268));
        }

        [StructLayout(LayoutKind.Sequential, Pack=8)]
        private struct ValvePackingSentinel_t
        {
            private uint m_u32;
            private ulong m_u64;
            private ushort m_u16;
            private double m_d;
        }
    }
}

