namespace Steamworks
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public sealed class Callback<T> : IDisposable
    {
        private CCallbackBaseVTable VTable;
        private IntPtr m_pVTable;
        private CCallbackBase m_CCallbackBase;
        private GCHandle m_pCCallbackBase;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DispatchDelegate<T> m_Func;
        private bool m_bGameServer;
        private readonly int m_size;
        private bool m_bDisposed;

        private event DispatchDelegate<T> m_Func
        {
            add
            {
                DispatchDelegate<T> func = this.m_Func;
                while (true)
                {
                    DispatchDelegate<T> objB = func;
                    func = Interlocked.CompareExchange<DispatchDelegate<T>>(ref this.m_Func, objB + value, func);
                    if (ReferenceEquals(func, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                DispatchDelegate<T> func = this.m_Func;
                while (true)
                {
                    DispatchDelegate<T> objB = func;
                    func = Interlocked.CompareExchange<DispatchDelegate<T>>(ref this.m_Func, objB - value, func);
                    if (ReferenceEquals(func, objB))
                    {
                        return;
                    }
                }
            }
        }

        public Callback(DispatchDelegate<T> func, bool bGameServer = false)
        {
            this.m_pVTable = IntPtr.Zero;
            this.m_size = Marshal.SizeOf(typeof(T));
            this.m_bGameServer = bGameServer;
            this.BuildCCallbackBase();
            this.Register(func);
        }

        private void BuildCCallbackBase()
        {
            CCallbackBaseVTable table = new CCallbackBaseVTable {
                m_RunCallResult = new CCallbackBaseVTable.RunCRDel(this.OnRunCallResult),
                m_RunCallback = new CCallbackBaseVTable.RunCBDel(this.OnRunCallback),
                m_GetCallbackSizeBytes = new CCallbackBaseVTable.GetCallbackSizeBytesDel(this.OnGetCallbackSizeBytes)
            };
            this.VTable = table;
            this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CCallbackBaseVTable)));
            Marshal.StructureToPtr(this.VTable, this.m_pVTable, false);
            CCallbackBase base2 = new CCallbackBase {
                m_vfptr = this.m_pVTable,
                m_nCallbackFlags = 0,
                m_iCallback = CallbackIdentities.GetCallbackIdentity(typeof(T))
            };
            this.m_CCallbackBase = base2;
            this.m_pCCallbackBase = GCHandle.Alloc(this.m_CCallbackBase, GCHandleType.Pinned);
        }

        public static Callback<T> Create(DispatchDelegate<T> func) => 
            new Callback<T>(func, false);

        public static Callback<T> CreateGameServer(DispatchDelegate<T> func) => 
            new Callback<T>(func, true);

        public void Dispose()
        {
            if (!this.m_bDisposed)
            {
                GC.SuppressFinalize(this);
                this.Unregister();
                if (this.m_pVTable != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(this.m_pVTable);
                }
                if (this.m_pCCallbackBase.IsAllocated)
                {
                    this.m_pCCallbackBase.Free();
                }
                this.m_bDisposed = true;
            }
        }

        ~Callback()
        {
            this.Dispose();
        }

        private int OnGetCallbackSizeBytes() => 
            this.m_size;

        private void OnRunCallback(IntPtr pvParam)
        {
            try
            {
                this.m_Func((T) Marshal.PtrToStructure(pvParam, typeof(T)));
            }
            catch (Exception exception1)
            {
                CallbackDispatcher.ExceptionHandler(exception1);
            }
        }

        private void OnRunCallResult(IntPtr pvParam, bool bFailed, ulong hSteamAPICall)
        {
            try
            {
                this.m_Func((T) Marshal.PtrToStructure(pvParam, typeof(T)));
            }
            catch (Exception exception1)
            {
                CallbackDispatcher.ExceptionHandler(exception1);
            }
        }

        public void Register(DispatchDelegate<T> func)
        {
            if (func == null)
            {
                throw new Exception("Callback function must not be null.");
            }
            if ((this.m_CCallbackBase.m_nCallbackFlags & 1) == 1)
            {
                this.Unregister();
            }
            if (this.m_bGameServer)
            {
                this.SetGameserverFlag();
            }
            this.m_Func = func;
            NativeMethods.SteamAPI_RegisterCallback(this.m_pCCallbackBase.AddrOfPinnedObject(), CallbackIdentities.GetCallbackIdentity(typeof(T)));
        }

        public void SetGameserverFlag()
        {
            this.m_CCallbackBase.m_nCallbackFlags = (byte) (this.m_CCallbackBase.m_nCallbackFlags | 2);
        }

        public void Unregister()
        {
            NativeMethods.SteamAPI_UnregisterCallback(this.m_pCCallbackBase.AddrOfPinnedObject());
        }

        public delegate void DispatchDelegate(T param);
    }
}

