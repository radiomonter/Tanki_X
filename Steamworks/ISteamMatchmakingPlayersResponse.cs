namespace Steamworks
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class ISteamMatchmakingPlayersResponse
    {
        private VTable m_VTable;
        private IntPtr m_pVTable;
        private GCHandle m_pGCHandle;
        private AddPlayerToList m_AddPlayerToList;
        private PlayersFailedToRespond m_PlayersFailedToRespond;
        private PlayersRefreshComplete m_PlayersRefreshComplete;

        public ISteamMatchmakingPlayersResponse(AddPlayerToList onAddPlayerToList, PlayersFailedToRespond onPlayersFailedToRespond, PlayersRefreshComplete onPlayersRefreshComplete)
        {
            if ((onAddPlayerToList == null) || ((onPlayersFailedToRespond == null) || (onPlayersRefreshComplete == null)))
            {
                throw new ArgumentNullException();
            }
            this.m_AddPlayerToList = onAddPlayerToList;
            this.m_PlayersFailedToRespond = onPlayersFailedToRespond;
            this.m_PlayersRefreshComplete = onPlayersRefreshComplete;
            VTable table = new VTable {
                m_VTAddPlayerToList = new InternalAddPlayerToList(this.InternalOnAddPlayerToList),
                m_VTPlayersFailedToRespond = new InternalPlayersFailedToRespond(this.InternalOnPlayersFailedToRespond),
                m_VTPlayersRefreshComplete = new InternalPlayersRefreshComplete(this.InternalOnPlayersRefreshComplete)
            };
            this.m_VTable = table;
            this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VTable)));
            Marshal.StructureToPtr(this.m_VTable, this.m_pVTable, false);
            this.m_pGCHandle = GCHandle.Alloc(this.m_pVTable, GCHandleType.Pinned);
        }

        ~ISteamMatchmakingPlayersResponse()
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

        private void InternalOnAddPlayerToList(IntPtr pchName, int nScore, float flTimePlayed)
        {
            this.m_AddPlayerToList(InteropHelp.PtrToStringUTF8(pchName), nScore, flTimePlayed);
        }

        private void InternalOnPlayersFailedToRespond()
        {
            this.m_PlayersFailedToRespond();
        }

        private void InternalOnPlayersRefreshComplete()
        {
            this.m_PlayersRefreshComplete();
        }

        public static explicit operator IntPtr(ISteamMatchmakingPlayersResponse that) => 
            that.m_pGCHandle.AddrOfPinnedObject();

        public delegate void AddPlayerToList(string pchName, int nScore, float flTimePlayed);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void InternalAddPlayerToList(IntPtr pchName, int nScore, float flTimePlayed);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void InternalPlayersFailedToRespond();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void InternalPlayersRefreshComplete();

        public delegate void PlayersFailedToRespond();

        public delegate void PlayersRefreshComplete();

        [StructLayout(LayoutKind.Sequential)]
        private class VTable
        {
            [NonSerialized, MarshalAs(UnmanagedType.FunctionPtr)]
            public ISteamMatchmakingPlayersResponse.InternalAddPlayerToList m_VTAddPlayerToList;
            [NonSerialized, MarshalAs(UnmanagedType.FunctionPtr)]
            public ISteamMatchmakingPlayersResponse.InternalPlayersFailedToRespond m_VTPlayersFailedToRespond;
            [NonSerialized, MarshalAs(UnmanagedType.FunctionPtr)]
            public ISteamMatchmakingPlayersResponse.InternalPlayersRefreshComplete m_VTPlayersRefreshComplete;
        }
    }
}

