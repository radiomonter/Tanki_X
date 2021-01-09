namespace Steamworks
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public sealed class CallResult<T> : IDisposable
    {
        private CCallbackBaseVTable VTable;
        private IntPtr m_pVTable;
        private CCallbackBase m_CCallbackBase;
        private GCHandle m_pCCallbackBase;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private APIDispatchDelegate<T> m_Func;
        private SteamAPICall_t m_hAPICall;
        private readonly int m_size;
        private bool m_bDisposed;

        private event APIDispatchDelegate<T> m_Func
        {
            add
            {
                APIDispatchDelegate<T> func = this.m_Func;
                while (true)
                {
                    APIDispatchDelegate<T> objB = func;
                    func = Interlocked.CompareExchange<APIDispatchDelegate<T>>(ref this.m_Func, objB + value, func);
                    if (ReferenceEquals(func, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                APIDispatchDelegate<T> func = this.m_Func;
                while (true)
                {
                    APIDispatchDelegate<T> objB = func;
                    func = Interlocked.CompareExchange<APIDispatchDelegate<T>>(ref this.m_Func, objB - value, func);
                    if (ReferenceEquals(func, objB))
                    {
                        return;
                    }
                }
            }
        }

        public CallResult(APIDispatchDelegate<T> func = null)
        {
            this.m_pVTable = IntPtr.Zero;
            this.m_hAPICall = SteamAPICall_t.Invalid;
            this.m_size = Marshal.SizeOf(typeof(T));
            this.m_Func = func;
            this.BuildCCallbackBase();
        }

        private void BuildCCallbackBase()
        {
            CCallbackBaseVTable table = new CCallbackBaseVTable {
                m_RunCallback = new CCallbackBaseVTable.RunCBDel(this.OnRunCallback),
                m_RunCallResult = new CCallbackBaseVTable.RunCRDel(this.OnRunCallResult),
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

        public void Cancel()
        {
            if (this.m_hAPICall != SteamAPICall_t.Invalid)
            {
                NativeMethods.SteamAPI_UnregisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong) this.m_hAPICall);
                this.m_hAPICall = SteamAPICall_t.Invalid;
            }
        }

        public static CallResult<T> Create(APIDispatchDelegate<T> func = null) => 
            new CallResult<T>(func);

        public void Dispose()
        {
            if (!this.m_bDisposed)
            {
                GC.SuppressFinalize(this);
                this.Cancel();
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

        ~CallResult()
        {
            this.Dispose();
        }

        public bool IsActive() => 
            this.m_hAPICall != SteamAPICall_t.Invalid;

        private int OnGetCallbackSizeBytes() => 
            this.m_size;

        private void OnRunCallback(IntPtr pvParam)
        {
            this.m_hAPICall = SteamAPICall_t.Invalid;
            try
            {
                this.m_Func((T) Marshal.PtrToStructure(pvParam, typeof(T)), false);
            }
            catch (Exception exception1)
            {
                CallbackDispatcher.ExceptionHandler(exception1);
            }
        }

        private void OnRunCallResult(IntPtr pvParam, bool bFailed, ulong hSteamAPICall)
        {
            SteamAPICall_t _t = (SteamAPICall_t) hSteamAPICall;
            if (_t == this.m_hAPICall)
            {
                try
                {
                    this.m_Func((T) Marshal.PtrToStructure(pvParam, typeof(T)), bFailed);
                }
                catch (Exception exception1)
                {
                    CallbackDispatcher.ExceptionHandler(exception1);
                }
                if (_t == this.m_hAPICall)
                {
                    this.m_hAPICall = SteamAPICall_t.Invalid;
                }
            }
        }

        public void Set(SteamAPICall_t hAPICall, APIDispatchDelegate<T> func = null)
        {
            if (func != null)
            {
                this.m_Func = func;
            }
            if (this.m_Func == null)
            {
                throw new Exception("CallResult function was null, you must either set it in the CallResult Constructor or in Set()");
            }
            if (this.m_hAPICall != SteamAPICall_t.Invalid)
            {
                NativeMethods.SteamAPI_UnregisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong) this.m_hAPICall);
            }
            this.m_hAPICall = hAPICall;
            if (hAPICall != SteamAPICall_t.Invalid)
            {
                NativeMethods.SteamAPI_RegisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong) hAPICall);
            }
        }

        public void SetGameserverFlag()
        {
            this.m_CCallbackBase.m_nCallbackFlags = (byte) (this.m_CCallbackBase.m_nCallbackFlags | 2);
        }

        public SteamAPICall_t Handle =>
            this.m_hAPICall;

        public delegate void APIDispatchDelegate(T param, bool bIOFailure);
    }
}

