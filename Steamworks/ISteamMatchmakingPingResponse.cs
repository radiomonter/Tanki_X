namespace Steamworks
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class ISteamMatchmakingPingResponse
    {
        private VTable m_VTable;
        private IntPtr m_pVTable;
        private GCHandle m_pGCHandle;
        private ServerResponded m_ServerResponded;
        private ServerFailedToRespond m_ServerFailedToRespond;

        public ISteamMatchmakingPingResponse(ServerResponded onServerResponded, ServerFailedToRespond onServerFailedToRespond)
        {
            if ((onServerResponded == null) || (onServerFailedToRespond == null))
            {
                throw new ArgumentNullException();
            }
            this.m_ServerResponded = onServerResponded;
            this.m_ServerFailedToRespond = onServerFailedToRespond;
            VTable table = new VTable {
                m_VTServerResponded = new InternalServerResponded(this.InternalOnServerResponded),
                m_VTServerFailedToRespond = new InternalServerFailedToRespond(this.InternalOnServerFailedToRespond)
            };
            this.m_VTable = table;
            this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VTable)));
            Marshal.StructureToPtr(this.m_VTable, this.m_pVTable, false);
            this.m_pGCHandle = GCHandle.Alloc(this.m_pVTable, GCHandleType.Pinned);
        }

        ~ISteamMatchmakingPingResponse()
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

        private void InternalOnServerFailedToRespond()
        {
            this.m_ServerFailedToRespond();
        }

        private void InternalOnServerResponded(gameserveritem_t server)
        {
            this.m_ServerResponded(server);
        }

        public static explicit operator IntPtr(ISteamMatchmakingPingResponse that) => 
            that.m_pGCHandle.AddrOfPinnedObject();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void InternalServerFailedToRespond();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void InternalServerResponded(gameserveritem_t server);

        public delegate void ServerFailedToRespond();

        public delegate void ServerResponded(gameserveritem_t server);

        [StructLayout(LayoutKind.Sequential)]
        private class VTable
        {
            [NonSerialized, MarshalAs(UnmanagedType.FunctionPtr)]
            public ISteamMatchmakingPingResponse.InternalServerResponded m_VTServerResponded;
            [NonSerialized, MarshalAs(UnmanagedType.FunctionPtr)]
            public ISteamMatchmakingPingResponse.InternalServerFailedToRespond m_VTServerFailedToRespond;
        }
    }
}

