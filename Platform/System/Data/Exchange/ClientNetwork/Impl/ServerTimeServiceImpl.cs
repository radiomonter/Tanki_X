namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class ServerTimeServiceImpl : ServerTimeService, ServerTimeServiceInternal
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Action<long> OnInitServerTime;
        private long initialServerTime;

        public event Action<long> OnInitServerTime
        {
            add
            {
                Action<long> onInitServerTime = this.OnInitServerTime;
                while (true)
                {
                    Action<long> objB = onInitServerTime;
                    onInitServerTime = Interlocked.CompareExchange<Action<long>>(ref this.OnInitServerTime, objB + value, onInitServerTime);
                    if (ReferenceEquals(onInitServerTime, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                Action<long> onInitServerTime = this.OnInitServerTime;
                while (true)
                {
                    Action<long> objB = onInitServerTime;
                    onInitServerTime = Interlocked.CompareExchange<Action<long>>(ref this.OnInitServerTime, objB - value, onInitServerTime);
                    if (ReferenceEquals(onInitServerTime, objB))
                    {
                        return;
                    }
                }
            }
        }

        public long InitialServerTime
        {
            get => 
                this.initialServerTime;
            set
            {
                this.initialServerTime = value;
                if (this.OnInitServerTime != null)
                {
                    this.OnInitServerTime(value);
                }
            }
        }
    }
}

