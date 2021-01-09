namespace Steamworks
{
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;

    public class InteropHelp
    {
        public static string PtrToStringUTF8(IntPtr nativeUtf8)
        {
            if (nativeUtf8 == IntPtr.Zero)
            {
                return null;
            }
            int ofs = 0;
            while (Marshal.ReadByte(nativeUtf8, ofs) != 0)
            {
                ofs++;
            }
            if (ofs == 0)
            {
                return string.Empty;
            }
            byte[] destination = new byte[ofs];
            Marshal.Copy(nativeUtf8, destination, 0, destination.Length);
            return Encoding.UTF8.GetString(destination);
        }

        public static void TestIfAvailableClient()
        {
            TestIfPlatformSupported();
            if (NativeMethods.SteamClient() == IntPtr.Zero)
            {
                throw new InvalidOperationException("Steamworks is not initialized.");
            }
        }

        public static void TestIfAvailableGameServer()
        {
            TestIfPlatformSupported();
            if (NativeMethods.SteamGameServerClient() == IntPtr.Zero)
            {
                throw new InvalidOperationException("Steamworks is not initialized.");
            }
        }

        public static void TestIfPlatformSupported()
        {
        }

        public class SteamParamStringArray
        {
            private IntPtr[] m_Strings;
            private IntPtr m_ptrStrings;
            private IntPtr m_pSteamParamStringArray;

            public SteamParamStringArray(IList<string> strings)
            {
                if (strings == null)
                {
                    this.m_pSteamParamStringArray = IntPtr.Zero;
                }
                else
                {
                    this.m_Strings = new IntPtr[strings.Count];
                    for (int i = 0; i < strings.Count; i++)
                    {
                        byte[] bytes = new byte[Encoding.UTF8.GetByteCount(strings[i]) + 1];
                        Encoding.UTF8.GetBytes(strings[i], 0, strings[i].Length, bytes, 0);
                        this.m_Strings[i] = Marshal.AllocHGlobal(bytes.Length);
                        Marshal.Copy(bytes, 0, this.m_Strings[i], bytes.Length);
                    }
                    this.m_ptrStrings = Marshal.AllocHGlobal((int) (Marshal.SizeOf(typeof(IntPtr)) * this.m_Strings.Length));
                    SteamParamStringArray_t structure = new SteamParamStringArray_t {
                        m_ppStrings = this.m_ptrStrings,
                        m_nNumStrings = this.m_Strings.Length
                    };
                    Marshal.Copy(this.m_Strings, 0, structure.m_ppStrings, this.m_Strings.Length);
                    this.m_pSteamParamStringArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SteamParamStringArray_t)));
                    Marshal.StructureToPtr(structure, this.m_pSteamParamStringArray, false);
                }
            }

            ~SteamParamStringArray()
            {
                IntPtr[] strings = this.m_Strings;
                int index = 0;
                while (true)
                {
                    if (index >= strings.Length)
                    {
                        if (this.m_ptrStrings != IntPtr.Zero)
                        {
                            Marshal.FreeHGlobal(this.m_ptrStrings);
                        }
                        if (this.m_pSteamParamStringArray != IntPtr.Zero)
                        {
                            Marshal.FreeHGlobal(this.m_pSteamParamStringArray);
                        }
                        break;
                    }
                    IntPtr hglobal = strings[index];
                    Marshal.FreeHGlobal(hglobal);
                    index++;
                }
            }

            public static implicit operator IntPtr(InteropHelp.SteamParamStringArray that) => 
                that.m_pSteamParamStringArray;
        }

        public class UTF8StringHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            public UTF8StringHandle(string str) : base(true)
            {
                if (str == null)
                {
                    base.SetHandle(IntPtr.Zero);
                }
                else
                {
                    byte[] bytes = new byte[Encoding.UTF8.GetByteCount(str) + 1];
                    Encoding.UTF8.GetBytes(str, 0, str.Length, bytes, 0);
                    IntPtr destination = Marshal.AllocHGlobal(bytes.Length);
                    Marshal.Copy(bytes, 0, destination, bytes.Length);
                    base.SetHandle(destination);
                }
            }

            protected override bool ReleaseHandle()
            {
                if (!this.IsInvalid)
                {
                    Marshal.FreeHGlobal(base.handle);
                }
                return true;
            }
        }
    }
}

