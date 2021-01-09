namespace Steamworks
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class ISteamMatchmakingRulesResponse
    {
        private VTable m_VTable;
        private IntPtr m_pVTable;
        private GCHandle m_pGCHandle;
        private RulesResponded m_RulesResponded;
        private RulesFailedToRespond m_RulesFailedToRespond;
        private RulesRefreshComplete m_RulesRefreshComplete;

        public ISteamMatchmakingRulesResponse(RulesResponded onRulesResponded, RulesFailedToRespond onRulesFailedToRespond, RulesRefreshComplete onRulesRefreshComplete)
        {
            if ((onRulesResponded == null) || ((onRulesFailedToRespond == null) || (onRulesRefreshComplete == null)))
            {
                throw new ArgumentNullException();
            }
            this.m_RulesResponded = onRulesResponded;
            this.m_RulesFailedToRespond = onRulesFailedToRespond;
            this.m_RulesRefreshComplete = onRulesRefreshComplete;
            VTable table = new VTable {
                m_VTRulesResponded = new InternalRulesResponded(this.InternalOnRulesResponded),
                m_VTRulesFailedToRespond = new InternalRulesFailedToRespond(this.InternalOnRulesFailedToRespond),
                m_VTRulesRefreshComplete = new InternalRulesRefreshComplete(this.InternalOnRulesRefreshComplete)
            };
            this.m_VTable = table;
            this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VTable)));
            Marshal.StructureToPtr(this.m_VTable, this.m_pVTable, false);
            this.m_pGCHandle = GCHandle.Alloc(this.m_pVTable, GCHandleType.Pinned);
        }

        ~ISteamMatchmakingRulesResponse()
        {
            if (this.m_pVTable != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(this.m_pVTable);
            }
            if (this.m_pGCHandle.IsAllocated)
            {
                this.m_pGCHandle.Free();
            }
        }

        private void InternalOnRulesFailedToRespond()
        {
            this.m_RulesFailedToRespond();
        }

        private void InternalOnRulesRefreshComplete()
        {
            this.m_RulesRefreshComplete();
        }

        private void InternalOnRulesResponded(IntPtr pchRule, IntPtr pchValue)
        {
            this.m_RulesResponded(InteropHelp.PtrToStringUTF8(pchRule), InteropHelp.PtrToStringUTF8(pchValue));
        }

        public static explicit operator IntPtr(ISteamMatchmakingRulesResponse that) => 
            that.m_pGCHandle.AddrOfPinnedObject();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void InternalRulesFailedToRespond();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void InternalRulesRefreshComplete();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void InternalRulesResponded(IntPtr pchRule, IntPtr pchValue);

        public delegate void RulesFailedToRespond();

        public delegate void RulesRefreshComplete();

        public delegate void RulesResponded(string pchRule, string pchValue);

        [StructLayout(LayoutKind.Sequential)]
        private class VTable
        {
            [NonSerialized, MarshalAs(UnmanagedType.FunctionPtr)]
            public ISteamMatchmakingRulesResponse.InternalRulesResponded m_VTRulesResponded;
            [NonSerialized, MarshalAs(UnmanagedType.FunctionPtr)]
            public ISteamMatchmakingRulesResponse.InternalRulesFailedToRespond m_VTRulesFailedToRespond;
            [NonSerialized, MarshalAs(UnmanagedType.FunctionPtr)]
            public ISteamMatchmakingRulesResponse.InternalRulesRefreshComplete m_VTRulesRefreshComplete;
        }
    }
}

