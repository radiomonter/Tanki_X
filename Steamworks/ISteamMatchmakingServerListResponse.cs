namespace Steamworks
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class ISteamMatchmakingServerListResponse
    {
        private VTable m_VTable;
        private IntPtr m_pVTable;
        private GCHandle m_pGCHandle;
        private ServerResponded m_ServerResponded;
        private ServerFailedToRespond m_ServerFailedToRespond;
        private RefreshComplete m_RefreshComplete;

        public ISteamMatchmakingServerListResponse(ServerResponded onServerResponded, ServerFailedToRespond onServerFailedToRespond, RefreshComplete onRefreshComplete)
        {
            if ((onServerResponded == null) || ((onServerFailedToRespond == null) || (onRefreshComplete == null)))
            {
                throw new ArgumentNullException();
            }
            this.m_ServerResponded = onServerResponded;
            this.m_ServerFailedToRespond = onServerFailedToRespond;
            this.m_RefreshComplete = onRefreshComplete;
            VTable table = new VTable {
                m_VTServerResponded = new InternalServerResponded(this.InternalOnServerResponded),
                m_VTServerFailedToRespond = new InternalServerFailedToRespond(this.InternalOnServerFailedToRespond),
                m_VTRefreshComplete = new InternalRefreshComplete(this.InternalOnRefreshComplete)
            };
            this.m_VTable = table;
            this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VTable)));
            Marshal.StructureToPtr(this.m_VTable, this.m_pVTable, false);
            this.m_pGCHandle = GCHandle.Alloc(this.m_pVTable, GCHandleType.Pinned);
        }

        ~ISteamMatchmakingServerListResponse()
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

        private void InternalOnRefreshComplete(HServerListRequest hRequest, EMatchMakingServerResponse response)
        {
            this.m_RefreshComplete(hRequest, response);
        }

        private void InternalOnServerFailedToRespond(HServerListRequest hRequest, int iServer)
        {
            this.m_ServerFailedToRespond(hRequest, iServer);
        }

        private void InternalOnServerResponded(HServerListRequest hRequest, int iServer)
        {
            this.m_ServerResponded(hRequest, iServer);
        }

        public static explicit operator IntPtr(ISteamMatchmakingServerListResponse that) => 
            that.m_pGCHandle.AddrOfPinnedObject();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void InternalRefreshComplete(HServerListRequest hRequest, EMatchMakingServerResponse response);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void InternalServerFailedToRespond(HServerListRequest hRequest, int iServer);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void InternalServerResponded(HServerListRequest hRequest, int iServer);

        public delegate void RefreshComplete(HServerListRequest hRequest, EMatchMakingServerResponse response);

        public delegate void ServerFailedToRespond(HServerListRequest hRequest, int iServer);

        public delegate void ServerResponded(HServerListRequest hRequest, int iServer);

        [StructLayout(LayoutKind.Sequential)]
        private class VTable
        {
            [NonSerialized, MarshalAs(UnmanagedType.FunctionPtr)]
            public ISteamMatchmakingServerListResponse.InternalServerResponded m_VTServerResponded;
            [NonSerialized, MarshalAs(UnmanagedType.FunctionPtr)]
            public ISteamMatchmakingServerListResponse.InternalServerFailedToRespond m_VTServerFailedToRespond;
            [NonSerialized, MarshalAs(UnmanagedType.FunctionPtr)]
            public ISteamMatchmakingServerListResponse.InternalRefreshComplete m_VTRefreshComplete;
        }
    }
}

